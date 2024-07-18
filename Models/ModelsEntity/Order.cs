using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("Order", Schema = "dbo")]
    public class Order
    {
        public Order() { }

        [Key]
        public int Id { get; set; }
        public string TitleName { get; set; }
        public string City {  get; set; }
        public string Adress { get; set; }
        public string Description {  get; set; }
        public int ActivTime { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("Performer")]
        public int PerformerId { get; set; }
        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        [ForeignKey("OrderPriority")]
        public int OrderPriorityId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set;} = DateTime.Now;
        public DateTime DeletedDate { get; set; }
    }
}
