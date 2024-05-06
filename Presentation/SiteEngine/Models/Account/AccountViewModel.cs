using System.ComponentModel.DataAnnotations;

namespace SiteEngine.Models.Account
{
    public class AccountViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения !!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения !!!")]
        public string PasswordHash { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения !!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения !!!")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения !!!")]
        [Compare("PasswordHash", ErrorMessage = "Пароли не совпадают !!!")]
        public string RepeatPasswordHash { get; set; }
    }
}
