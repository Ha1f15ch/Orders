using System.ComponentModel.DataAnnotations;

namespace ModelsEntity;

public class Service
{
    public Service() { }

    [Key]
    public int Id { get; set; }
    public string NameOfService { get; set; } = "Название услуги";
}
