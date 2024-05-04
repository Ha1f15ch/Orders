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
        public DateTime DateOfBirth { get; set; }
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
        public string Experience { get; set; }
        public string Education { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }

    }
}
