using System.ComponentModel.DataAnnotations;

namespace SiteEngine.Models.Amplua
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PhoneNumberValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }

            if (value.ToString().Length != 11)
            {
                return false;
            }

            if (!value.ToString().All(char.IsDigit))
            {
                return false;
            }

            if (value.ToString().Contains(" "))
            {
                return false;
            }

            return true;
        }
    }
}
