using ApplicationDbContext.Interfaces.ServicesInterfaces;
using Azure.Core;

namespace ApplicationDbContext.ContextRepositories.Services
{
    public class ServiceGetCookieData : IServiceInterfaceGetCookieData
    {
        /*public int GetUserIdFromCookie()
        {
            if (Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if (int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }
            return 0;
        }*/
        public int GetUserIdFromCookie()
        {
            throw new NotImplementedException();
        }
    }
}
