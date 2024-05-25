using ApplicationDbContext;
using Microsoft.AspNetCore.Mvc;
using ModelsEntity;

namespace SiteEngine.Controllers
{
    public class AmpluaController : BaseController
    {
        private readonly AppDbContext context;

        public AmpluaController(AppDbContext appDbContext)
        {
            this.context = appDbContext;
        }

        private int GetUserIdFromCookie()
        {
            if(Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if(int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }

            return 0;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {

            User user = await this.context.Users.FindAsync(GetUserIdFromCookie());
            
            if (user == null)
            {
                return View();
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProfileCustomerAsync()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProfilePerformerAsync()
        {
            return View();
        }


    }
}
