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
        public void UpdateStatusOrder(Order order, int statusId);
        public void DeleteOrderById(int orderId);//будем ставить статус отмены и дату удаления
        public void UpdatePerformer(Order order, int performerId);
        public void UpdatePriorityOrder(Order order, int priorityId);
        public Task<List<Order>> GetOrderByDate(DateOnly date);
        public Task<List<Order>> GetOrderByPriorityId(int priorityId);
        public Task<List<Order>> GetOrderByStatusId(int statusId);
        public Task<Order> GetOrderById(int orderId);
    }
}
