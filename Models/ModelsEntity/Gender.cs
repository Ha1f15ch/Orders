using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class Gender
    {
        public Gender(string name)
        {
            this.Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
