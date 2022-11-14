using AutoMapper;
using Hangfire;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _jobClient;

        private IPaymentStrategy _strategy;

        public PaymentMethodService(IUnitOfWork unitOfWork, IOrderService orderService, IMapper mapper, IBackgroundJobClient jobClient, IPaymentStrategy paymentStrategy = null)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _strategy = paymentStrategy;
            _jobClient = jobClient;
        }

        public async Task<IEnumerable<PaymentMethodModel>> GetAllAsync()
        {
            var paymentMethodEntities = await _unitOfWork.PaymentMethodRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentMethodModel>>(paymentMethodEntities);
        }

        public void SetPaymentMethod(IPaymentStrategy paymentStrategy)
        {
            _strategy = paymentStrategy;
        }

        public async Task PayAsync(Guid userId, object paymentInfo)
        {
            var order = await _orderService.GetLastOrderByUserIdAsync(userId);
            try
            {
                if (order.OrderState == OrderState.Canceled)
                {
                    throw new ModelException("Time for payment is expired");
                }
                else if (order.OrderState != OrderState.PaymentPending)
                {
                    throw new ModelException("Order is not pending for payment");
                }
                else
                {
                    bool isPaymentSuccessful = _strategy.Pay(paymentInfo);

                    if (isPaymentSuccessful)
                    {
                        await _orderService.ChangeOrderState(order, OrderState.Paid);
                    }
                    else
                    {
                        throw new ModelException("Payment is failed");
                    }
                }
            }
            catch (Exception)
            {
                await _orderService.ReturnGamesInStock(order.Items);
                await _orderService.ChangeOrderState(order, OrderState.Opened);
                throw;
            }
        }

        public async Task StartTimeout(Guid customerId, int timeoutMinutes)
        {
            var order = await _orderService.GetLastOrderByUserIdAsync(customerId);
            await _orderService.ChangeOrderState(order, OrderState.PaymentPending);
            await _orderService.TakeAwaySoldGames(order.Items);

            _jobClient.Schedule(() => _orderService.ManageOrderAfterTimeout(order), TimeSpan.FromMinutes(timeoutMinutes));
        }
    }
}
