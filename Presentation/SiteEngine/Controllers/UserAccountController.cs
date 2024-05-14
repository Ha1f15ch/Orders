using ApplicationDbContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsEntity;
using SiteEngine.Models.Account;
using System.Security.Claims;

namespace SiteEngine.Controllers
{
    public class UserAccountController : BaseController
    {
        private readonly AppDbContext context;

        public UserAccountController(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //Превью страницы для регистрации
        [HttpGet]
        public async Task<IActionResult> RegistrationAsync()
        {
            return View();
        }

        //Превью страницы для входа
        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            return View();
        }

        //Дейсвие выхода 
        public async Task <IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "MainPage");
        }

        //Аутентификация, определение доступа для пользователя
        private async Task AuthenticateAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        //Дейсвие login POST
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new AccountViewModel
                {
                    LoginViewModel = model
                });
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.PasswordHash == model.PasswordHash);

            if (user is null)
            {
                ViewBag.Error = "Некорректно введен логин или пароль !!!";
                return View("Index", new AccountViewModel
                {
                    LoginViewModel = model
                });
            }

            await AuthenticateAsync(user);
            return RedirectToAction("Index", "MainPage");
        }

        //Дейсвие Register POST
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new AccountViewModel
                {
                    RegisterViewModel = model
                });
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            if (user is not null)
            {
                ViewBag.RegisterError = "Пользователь с таким логином уже существует !!!";
                return View("Index", new AccountViewModel
                {
                    RegisterViewModel = model,
                });
            }

            user = new User(model.UserName, model.PasswordHash);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await AuthenticateAsync(user);
            return RedirectToAction("Index", "MainPage");
        }
    }
}
