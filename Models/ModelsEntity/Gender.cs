using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("Gender", Schema = "dict")]
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
