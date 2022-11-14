using OnlineGameStore.BLL.CustomValidator;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class VisaModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^4[0-9]{12}(?:[0-9]{3})?$")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}")]
        public string CVV2 { get; set; }

        [Required]
        [CardExpireValidation]
        [RegularExpression("[0-9]{2}/[0-9]{4}")]
        public string DateOfExpiry { get; set; }
    }
}
