using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class PublisherModel
    {
        public Guid Id { get; set; }

        [Required]
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
    }
}
