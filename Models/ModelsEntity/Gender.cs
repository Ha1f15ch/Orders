using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class Gender
    {
        public Gender() { }

        public Gender(string name)
        {
            this.Name = name;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public static int GetGenderIdForDefault()
        {
            return 1;
        }

        public static int GetGenderById(int id)
        {
            return id;
        }
    }
}
