using System.ComponentModel.DataAnnotations;

namespace SiteEngine.Models.Amplua
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CustomValidatorAttributes : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
    }
}
