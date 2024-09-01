using ModelsEntity;

namespace SiteEngine.Models.Order
{
    public class DetailOrderViewModelForPerformer
    {
        public ModelsEntity.Order Order { get; set; }
        public Customer? CustomerProfile { get; set; }
        public Performer? PerformerProfile { get; set; }
        public List<OrderPriority> OrderPriorities { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
        public bool HasRequestInQueue { get; set; }
    }
}
