using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsEntity;

[Table("Service", Schema = "dict")]
public class Service
{
    public Service() { }

    [Key]
    public int Id { get; set; }
    public string NameOfService { get; set; } = "Название услуги";
}
