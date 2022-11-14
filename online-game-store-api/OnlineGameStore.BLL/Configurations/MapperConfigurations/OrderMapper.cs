using AutoMapper;
using OnlineGameStore.BLL.Models;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.Entities;
using System;

namespace OnlineGameStore.BLL.Configurations.MapperConfigurations
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {

            CreateMap<OrderModel, Order>()
                .ForMember(orderModel => orderModel.ShippedDate,
                opt => opt.MapFrom(order => order.ShippedDate == null && order.OrderState == OrderState.Shipped ? DateTime.UtcNow : order.ShippedDate));


            CreateMap<Order, OrderModel>()
                .ForMember(orderModel => orderModel.OrderState,
                opt => opt.MapFrom(order => order.MongoCustomerId != null ? OrderState.Complete : order.OrderState));
        }
    }
}
