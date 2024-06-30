using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class Category
    {
        public Category() { }

        [Key]
        public int Id { get; set; }
        public string NameOfCategory { get; set; }
    }
}
