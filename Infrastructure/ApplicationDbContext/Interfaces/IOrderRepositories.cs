using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IOrderRepositories
    {
        public Task CreateNewOrder(Order order);
        public Task UpdateStatusOrder(int orderId, string statusId, int userId, bool isCustomer, bool isPerformer);
        public Task DeleteOrderById(int orderId);
        public Task UpdatePerformer(int orderId, int performerId);
        public Task UpdatePriorityOrder(int id, string name);
        public Task UpdateOrder(Order order);
        public Task CancelOrder(int orderId, bool? isCustomer, bool? isPerformer);
        public Task<List<Order>> GetOrderByCustomFilter(OrderFilterParams filterParams);
        public Task<Order> GetOrderById(int orderId);
    }
}
