using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("TypeListData", Schema = "meta")]
    public class TypeListData
    {   
        public TypeListData() { }

        [Key]
        
        public int Id { get; set; }
        [ForeignKey("Name")]
        public string Name { get; set; }
        [ForeignKey("Description")]
        public string Description { get; set; }
    }
}
