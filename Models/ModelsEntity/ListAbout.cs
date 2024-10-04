using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("ListAbout", Schema = "dbo")]
    public class ListAbout
    {
        public ListAbout() { }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<ListAboutItem> ListAboutItems { get; set; }
    }
}
