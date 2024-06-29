using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class Performer
    {
        public Performer() { }

        public Performer(string lastName, string firstName, string middleName, string phoneNumber, string city, float avgRating)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            City = city;
            AverageRating = avgRating;
        }

        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string City { get; set; }
        public string? Experience { get; set; }
        public string? Education { get; set; }
        public string? Description { get; set; }
        public double? AverageRating { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("User")]
        public int UserId { get; set; } 
        public List<PerformerServiceMapping>? PerformerServices { get; set; } = null;
    }
}
