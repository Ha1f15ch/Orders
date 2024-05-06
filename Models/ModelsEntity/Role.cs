using System.ComponentModel.DataAnnotations;

namespace ModelsEntity
{
    public class Role
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
