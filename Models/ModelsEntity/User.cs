using System.Diagnostics.CodeAnalysis;

namespace ModelsEntity
{
    public class User
    {
        public User(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }

        public int Id {  get; set; }
        public string UserName { get; set; }
        private int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;

                if (DateOfBirth.Date > today.AddYears(-age)) age--;

                return age;
            }
        }
        public string NormalizedUserName
        {
            get
            {
                return this.UserName.ToUpper();
            }
        }
        [AllowNull, MaybeNull] 
        public string? Email { get; set; } = string.Empty;
        [AllowNull, MaybeNull]
        public string? NormalizedEmail
        {
            get
            {
                if(this.Email != null)
                {
                    return this.Email.ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        [AllowNull, MaybeNull] 
        public bool? EmailConfirmed { get; set; }
        [AllowNull, MaybeNull] 
        public string? PasswordHash { get; set; }
        [AllowNull, MaybeNull] 
        public string? PhoneNumber { get; set; }
        [AllowNull, MaybeNull] 
        public bool? PhoneNumberConfirmed { get; set; }
        [AllowNull, MaybeNull] 
        public bool? TwoFactorEnabled { get; set; }
        public bool IsActiv { get; set; } = true;
        public bool IsCustomer { get; set; } = false;
        public bool IsPerformer { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public Customer Customer { get; set; }
        public Performer Performer { get; set; }
        public Gender Gender { get; set; } = new Gender("default value");
    }
}
