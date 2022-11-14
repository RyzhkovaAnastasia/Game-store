using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class IBoxModel
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public decimal Sum
        {
            get => Math.Round(_sum, 2);
            set => _sum = Math.Round(value, 2);
        }
        private decimal _sum;
    }
}
