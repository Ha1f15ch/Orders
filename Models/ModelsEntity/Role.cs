using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsEntity
{
    [Table("Role", Schema = "dict")]
    public class Role
    {
        public Role() { }

        [Key]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
