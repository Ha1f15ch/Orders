using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsEntity;

namespace ModelsEntity
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        public string? Address { get; set; }
        public int UserId { get; set;}
        public User User { get; set; }
    }
}
