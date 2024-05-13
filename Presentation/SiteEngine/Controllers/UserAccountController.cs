using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SiteEngine.Controllers
{
    public class UserAccountController : BaseController
    {
        //Превью страницы для регистрации
        [HttpGet]
        public Task <IActionResult> RegistrationAsyncGet()
        {
            return Task.FromResult<IActionResult>(View());
        }

        //Превью страницы для входа
        [HttpGet]
        public Task <IActionResult> LoginAsyncGet()
        {
            return Task.FromResult<IActionResult>(View());
        }

        //дейсвие выхода 
        public async Task <IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "MainPage");
        }
    }
}
