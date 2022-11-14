using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OnlineGameStore.BLL.Configurations.JWT
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int TokenLifetime { get; set; } //minutes

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
    }
}
