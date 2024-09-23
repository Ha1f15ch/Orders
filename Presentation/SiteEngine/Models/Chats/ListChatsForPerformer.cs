using ModelsEntity;

namespace SiteEngine.Models.Chats
{
    public class ListChatsForPerformer
    {
        public List<Chat>? ListChats { get; set; }
        public List<ModelsEntity.Order>? ListOrders { get; set; }
        public Performer? PerformerProfile { get; set; }
        public List<Performer>? ListPerformers { get; set; }
        public Customer? CustomerProfile { get; set; }
        public List<Customer>? ListCustomers { get; set; }
    }
}
