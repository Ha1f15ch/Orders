using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("CategoryProfessionMapping", Schema = "dict")]
    public class CategoryProfessionMapping
    {
        public CategoryProfessionMapping() { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("Profession")]
        public int ProfessionId { get; set; }
    }
}
