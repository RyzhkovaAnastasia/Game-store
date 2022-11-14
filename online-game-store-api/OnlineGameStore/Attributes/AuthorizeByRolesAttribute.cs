using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles) : base()
    {
        Roles = string.Join(",", roles);
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
