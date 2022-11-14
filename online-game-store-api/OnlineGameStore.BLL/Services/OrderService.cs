using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineGameStore.BLL.CustomExceptions;
using OnlineGameStore.BLL.Interfaces;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderModel> CreateAsync(OrderModel newOrder)
        {
            var orderEntity = _mapper.Map<Order>(newOrder);
            orderEntity = await _unitOfWork.OrderRepository.CreateAsync(orderEntity);

            return _mapper.Map<OrderModel>(orderEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.OrderRepository.SoftDeleteAsync(id);
        }

        public async Task<OrderModel> EditAsync(OrderModel updatedOrder)
        {
            var orderEntity = _mapper.Map<Order>(updatedOrder);
            orderEntity = await _unitOfWork.OrderRepository.EditAsync(orderEntity);

            if (orderEntity == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<OrderModel>(orderEntity);
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            var orderEntities = await _unitOfWork.OrderRepository.GetAllAsync();
            foreach (var order in orderEntities)
            {
                order.Items = (await _unitOfWork.OrderDetailRepository.GetAllAsync(od => od.OrderId == order.Id)).ToList();
            }

            return _mapper.Map<IEnumerable<OrderModel>>(orderEntities);
        }

        public async Task<OrderModel> GetByIdAsync(Guid id)
        {
            Order orderEntity = await _unitOfWork.OrderRepository.FindAsync(x => x.Id == id);
            if (orderEntity == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<OrderModel>(orderEntity);
        }

        public async Task<OrderModel> ChangeOrderState(OrderModel order, OrderState orderState)
        {
            order.OrderState = orderState;
            var orderEntity = await _unitOfWork.OrderRepository.EditFieldAsync(_mapper.Map<Order>(order), nameof(order.OrderState));

            if (orderEntity.OrderDate == null && orderEntity.OrderState == OrderState.Paid)
            {
                orderEntity.OrderDate = DateTime.UtcNow;
                await _unitOfWork.OrderRepository.EditFieldAsync(_mapper.Map<Order>(orderEntity), nameof(order.OrderDate));
            }
            else if (orderEntity.ShippedDate == null && orderEntity.OrderState == OrderState.Shipped)
            {
                orderEntity.ShippedDate = DateTime.UtcNow;
                await _unitOfWork.OrderRepository.EditFieldAsync(_mapper.Map<Order>(orderEntity), nameof(order.ShippedDate));
            }

            return _mapper.Map<OrderModel>(orderEntity);
        }

        public async Task<OrderModel> GetLastOrderByUserIdAsync(Guid userId)
        {
            var order = (await _unitOfWork.OrderRepository
                .GetAllAsync(x => x.UserId == userId &&
                (x.OrderState == OrderState.Opened || x.OrderState == OrderState.PaymentPending || x.OrderState == OrderState.Canceled)))
                .OrderByDescending(x => x.OrderDate)
                .FirstOrDefault();

            if (order == null)
            {
                return null;
            }

            order.Items = (await _unitOfWork.OrderDetailRepository.GetAllAsync(x => x.OrderId == order.Id)).ToList();

            order.Items?.ToList().ForEach(x => x.Game = _unitOfWork.GameRepository.FindAsync(g => g.Id == x.GameId).Result);

            return _mapper.Map<OrderModel>(order);
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ManageOrderAfterTimeout(OrderModel order)
        {
            var updatedOrder = await GetByIdAsync(order.Id);
            if (updatedOrder.OrderState == OrderState.PaymentPending)
            {
                await ChangeOrderState(order, OrderState.Canceled);
                await ReturnGamesInStock(order.Items);
            }
        }

        public async Task TakeAwaySoldGames(IEnumerable<OrderDetailModel> orderDetails)
        {
            IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadUncommitted);

            foreach (OrderDetailModel item in orderDetails)
            {
                Game game = await _unitOfWork.GameRepository.FindAsync(g => g.Id == item.GameId);

                if (game == null)
                {
                    await _unitOfWork.RollbackTransaction(transaction);
                    throw new NotFoundException("Game is not found");
                }

                try
                {
                    game.UnitsInStock -= item.Quantity;
                    await _unitOfWork.GameRepository.EditFieldAsync(game, nameof(game.UnitsInStock));
                }
                catch
                {
                    await _unitOfWork.RollbackTransaction(transaction);
                    throw new ModelException($"{ game.Name } is out of stock. Please check your basket for details");
                }
            }
            await _unitOfWork.CommitTransaction(transaction);
        }

        public async Task ReturnGamesInStock(IEnumerable<OrderDetailModel> orderDetails)
        {
            foreach (OrderDetailModel item in orderDetails)
            {
                Game game = await _unitOfWork.GameRepository.FindAsync(g => g.Id == item.GameId);

                if (game == null)
                {
                    throw new NotFoundException("Game is not found");
                }

                game.UnitsInStock += item.Quantity;
                await _unitOfWork.GameRepository.EditFieldAsync(game, nameof(game.UnitsInStock));
            }
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync(x => x.OrderDate >= startDate && x.OrderDate <= endDate);

            foreach (var order in orders)
            {
                order.Items = (await _unitOfWork.OrderDetailRepository.GetAllAsync(od => od.OrderId == order.Id)).ToList();
            }

            return _mapper.Map<IEnumerable<OrderModel>>(orders.OrderByDescending(x => x.OrderDate));
        }

        public async Task MergeBasketsAsync(Guid guestBasketId, Guid userBasketId)
        {
            var guestOrder = await GetLastOrderByUserIdAsync(guestBasketId);
            var userOrder = await GetLastOrderByUserIdAsync(userBasketId);

            if (!(guestOrder == null || !guestOrder.Items.Any() || userOrder == null))
            {

                foreach (var item in guestOrder.Items)
                {
                    if (!userOrder.Items.Select(x => x.GameId).Contains(item.GameId))
                    {
                        item.OrderId = userOrder.Id;
                        item.Id = Guid.NewGuid();
                        await _unitOfWork.OrderDetailRepository.CreateAsync(_mapper.Map<OrderDetail>(item));
                    }
                }
                await _unitOfWork.OrderRepository.DeleteAsync(guestOrder.Id);
            }
        }
    }
}
