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
        public Task FinishOrder(int orderId, int performerId);
        public Task DeleteOrderById(int orderId);
        public Task UpdatePerformer(int orderId, int performerId);
        public Task UpdatePriorityOrder(int id, string name);
        public Task UpdateOrder(Order order);
        public Task<List<Order>> GetOrderByCustomFilter(OrderFilterParams filterParams);
        public Task<Order> GetOrderById(int orderId);
        public Task<List<Order>> GetAllOrder();
        public Task<List<Order>> GetAllOrderByCustomerId(int customerId);
        public Task<List<Order>> GetAllOrderByPerformerId(int performerId);
    }
}
