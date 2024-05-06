using Microsoft.AspNetCore.Mvc;

namespace SiteEngine.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
