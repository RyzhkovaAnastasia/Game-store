using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace OnlineGameStore.Extensions
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserClaims
    {
        private ClaimsPrincipal Claims => _httpContextAccessor.HttpContext.User;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Guid Id => Guid.TryParse(Claims.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : GetGuestId();
        public Guid CookieGuestId => GetGuestId();
        public string Username => Claims.FindFirst(ClaimTypes.Name)?.Value;
        public string Email => Claims.FindFirst(ClaimTypes.Email)?.Value;
        public string Role => Claims.FindFirst(ClaimTypes.Role)?.Value;
        public Guid PublisherId => Guid.TryParse(Claims.FindFirst("publisherId")?.Value, out var id) ? id : Guid.Empty;

        public UserClaims(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserHasAccess(Guid publisherId)
        {
            return (Claims.IsInRole(Common.Enums.Role.Publisher) && PublisherId == publisherId) || !Claims.IsInRole(Common.Enums.Role.Publisher);
        }

        [AllowAnonymous]
        private Guid GetGuestId()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["guestId"] != null)
            {
                return Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies["guestId"]);
            }
            else
            {
                var guestId = Guid.NewGuid();
                _httpContextAccessor.HttpContext.Response.Cookies.Append("guestId", guestId.ToString(),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.MaxValue
                    });
                return guestId;
            }
        }
    }
}