using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class User
    {
        public int Id {  get; set; }
        public string UserName { get; set; }
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
