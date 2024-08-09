using ApplicationDbContext.Interfaces.ServicesInterfaces;
using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace ApplicationDbContext.ContextRepositories.Services
{
    public class ServiceGetCookieData : IServiceInterfaceGetCookieData
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServiceGetCookieData(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetUserIdFromCookie()
        {
            var context = httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if (int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }
            return 0;
        }
    }
}
