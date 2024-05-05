using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class Performer
    {
        public int Id { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int UserId { get; set; } 
        public User User { get; set; }
    }
}
