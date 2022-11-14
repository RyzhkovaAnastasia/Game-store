using System;

namespace OnlineGameStore.BLL.Models.AuthModels
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid? PublisherId { get; set; }
    }
}
