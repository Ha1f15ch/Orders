using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("OrderPerformerMapping", Schema = "dbo")]
    public class OrderPerformerMapping
    {
        public OrderPerformerMapping() { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Performer")]
        public int PerformerId { get; set; }
    }
}
