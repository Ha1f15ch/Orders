using ModelsEntity;

namespace SiteEngine.Models.Order
{
    public class CreateOrderViewModel
    {
        public Customer? CustomerProfile { get; set; }
        public List<OrderPriority>? OrderPriorities { get; set; }
        public OrderForCreateViewModel Order { get; set; }
    }
}
