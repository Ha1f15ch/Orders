using ModelsEntity;
using System.ComponentModel.DataAnnotations;

namespace SiteEngine.Models.Order
{
    public class OrderViewModel
    {
        public OrderForCreateViewModel orderForCreateViewModel { get; set; }
        public OrderForUpdateViewModel orderForUpdateViewModel { get; set; }
    }

    public class OrderForCreateViewModel
    {
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string TitleName { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Adress { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public int ActivTime { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string OrderPriority { get; set; }
    }

    public class OrderForUpdateViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения !!!")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string TitleName { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string Adress { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public int ActivTime { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Данное поле обязатлеьно для заполнения !!!")]
        public string OrderPriority { get; set; }
    }
}
