using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models.AuthModels
{
    public class SignUpModel
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
