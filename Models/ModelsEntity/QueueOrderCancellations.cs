using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("QueueOrderCancellations", Schema = "dbo")]
    public class QueueOrderCancellations
    {
        public QueueOrderCancellations() { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Performer")]
        public int? PerformerId { get; set; }
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public bool IsConfirmedByCustomer { get; set; } = false;
        public bool IsConfirmedByPerformer { get; set; } = false;
    }
}
