using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class ServiceCategoryMapping
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
