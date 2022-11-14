using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class PlatformTypeModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
