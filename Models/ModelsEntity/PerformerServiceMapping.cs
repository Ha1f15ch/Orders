using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class PerformerServiceMapping
    {
        public PerformerServiceMapping() { }

        public int Id { get; set; }
        public int PerformerId { get; set; }
        public Performer Performer { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
