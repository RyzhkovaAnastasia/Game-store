using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models.AuthModels
{
    public class SignInModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}