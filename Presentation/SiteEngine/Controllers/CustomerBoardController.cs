using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace SiteEngine.Controllers
{
    public class CustomerBoardController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;

        public CustomerBoardController(AppDbContext context, IProfileCustomerRepositories profileCustomerRepositories)
        {
            this.context = context;
            this.profileCustomerRepositories = profileCustomerRepositories;
        }

        private int GetUserIdFromCookie()
        {
            if (Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if (int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }
            return 0;
        }


        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(GetUserIdFromCookie());

            return View(myProfileCustomerModel);
        }
    }
}
