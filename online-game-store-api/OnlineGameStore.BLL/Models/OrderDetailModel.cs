using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class OrderDetailModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid GameId { get; set; }
        public GameModel Game { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public short Quantity { get; set; }
        public float Discount { get; set; }

        public Guid OrderId { get; set; }
    }
}
