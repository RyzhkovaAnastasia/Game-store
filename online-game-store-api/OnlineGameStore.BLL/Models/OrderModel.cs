using OnlineGameStore.Common.Enums;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.BLL.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public OrderState OrderState { get; set; } = OrderState.Opened;
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public Guid? ShipperId { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public IEnumerable<OrderDetailModel> Items { get; set; }
    }
}
