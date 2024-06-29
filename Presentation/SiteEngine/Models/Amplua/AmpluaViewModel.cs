using ModelsEntity;
using SiteEngine.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;

namespace SiteEngine.Models.Amplua
{
    public class AmpluaViewModel
    {
        public CustomerProfileViewModel CustomerProfileViewModel { get; set; }
        public PerformerProfileViewModel PerformerProfileViewModel { get; set; }
        public PerformerProfileFullViewModel PerformerProfileFullViewModel { get; set; }

        //public 
    }

    public class CustomerProfileViewModel : Customer
    {
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        [PhoneNumberValidator(ErrorMessage = "Номер должен состоять из 9 цифр и не содержать пробелов !!!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Adress { get; set; }
    }

    public class CustomerProfileFullViewModel : Customer
    {
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        [PhoneNumberValidator(ErrorMessage = "Номер должен состоять из 9 цифр и не содержать пробелов !!!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Adress { get; set; }
    }

    public class PerformerProfileViewModel : Performer
    {
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string City { get; set; }
    }

    public class PerformerProfileFullViewModel : Performer
    {
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        [PhoneNumberValidator(ErrorMessage = "Номер должен состоять из 9 цифр и не содержать пробелов !!!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для запорлненния !!!")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Description { get; set; }
    }
}
