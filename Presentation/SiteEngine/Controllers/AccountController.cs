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
    public class AccountController : BaseController
    {
        private readonly AppDbContext context;

        public AccountController(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoginAsyncGet()
        {
            return View();
        }

        public async Task<IActionResult> LoginAsync([Bind(Prefix = "l")] LoginViewModel model)
        {
            if(!ModelState.IsValid)
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
            return RedirectToAction("Index", "Home");
        }

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

        public async Task<IActionResult> RegisterAsync([Bind(Prefix = "r")] RegisterViewModel model)
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
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
