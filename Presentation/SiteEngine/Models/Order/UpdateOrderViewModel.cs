using ModelsEntity;

namespace SiteEngine.Models.Order
{
    public class UpdateOrderViewModel
    {
        public Customer? CustomerProfile { get; set; }
        public List<OrderPriority>? OrderPriorities { get; set; }
        public OrderForUpdateViewModel Order { get; set; }
    }
}
