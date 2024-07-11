using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsEntity;

namespace ModelsEntity
{
    [Table("Customer", Schema = "dbo")]
    public class Customer
    {
        public Customer() { }

        public Customer(string fName, string lName, string mName, string phone, string city, string adress/*, int userId*/)
        {
            FirstName = fName;
            LastName = lName;
            MiddleName = mName;
            PhoneNumber = phone;
            City = city;
            Adress = adress;
        }

        [Key]
        public int Id { get; set; }
        public string LastName { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set;}
    }
}
