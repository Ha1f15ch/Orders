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

        public static int GetGenderIdForDefault()
        {
            return 1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
