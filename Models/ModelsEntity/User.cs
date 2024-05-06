namespace ModelsEntity
{
    public class User
    {
        public int Id {  get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public int Age
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
        public string Email { get; set; }
        public string NormalizedEmail
        {
            get
            {
                return this.Email.ToUpper();
            }
        }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsActiv {  get; set; }
        public bool IsCustomer { get; set; }
        public bool IsPerformer { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Customer Customer { get; set; }
        public Performer Performer { get; set; }
    }
}
