using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    public class ProfessionServiceMapping
    {
        public ProfessionServiceMapping() { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        [ForeignKey("Profession")]
        public int ProfessionId { get; set; }
    }
}
