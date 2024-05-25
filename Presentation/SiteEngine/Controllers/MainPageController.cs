using Microsoft.AspNetCore.Mvc;

namespace SiteEngine.Controllers
{
    public class MainPageController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
