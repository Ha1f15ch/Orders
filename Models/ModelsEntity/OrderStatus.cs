using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("OrderStatus", Schema = "meta")]
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
