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
        public void CreateNewOrder(Order order);
        public void UpdateStatusOrder(int orderId, string statusId, int userId, bool isCustomer, bool isPerformer);
        public void DeleteOrderById(int orderId);
        public void UpdatePerformer(int orderId, int performerId);
        public void UpdatePriorityOrder(int id, string name);
        public void UpdateOrder(Order order);
        public Task<List<Order>> GetAllMyOrders(int userId, bool isCustomer, bool isPerformer);
        public Task<List<Order>> GetAllMyCompletedOrders(int userId, bool isCustomer, bool isPerformer);
        public Task<List<Order>> GetAllMyStartedOrders(int userId, bool isCustomer, bool isPerformer);
        public Task<List<Order>> GetAllMyCanceledOrders(int userId, bool isCustomer, bool isPerformer);
        public Task<List<Order>> GetOrderByCustomFilter(OrderFilterParams filterParams);
        public Task<Order> GetOrderById(int orderId);
    }
}
