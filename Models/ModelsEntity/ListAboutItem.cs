using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("ListAboutItem", Schema = "dbo")]
    public class ListAboutItem
    {   
        public ListAboutItem() { }

        [Key]
        public int Id { get; set; }
    
        public string Type { get; set; }
    }
}
