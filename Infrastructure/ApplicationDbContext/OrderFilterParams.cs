using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext
{
    public class OrderFilterParams
    {
        public DateOnly? DateCreateStart { get; set; }
        public DateOnly? DateCreateEnd { get; set; }
        public DateOnly? DateCanceledStart { get; set; }
        public DateOnly? DateCanceledEnd { get; set; }
        public string? StatusId { get; set; }
        public string? PriorityId { get; set; }
        public int UserId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsPerformer { get; set; }
    }
}
