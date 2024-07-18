using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class OrderRepositories : IOrderRepositories
    {
        private readonly AppDbContext context;

        public OrderRepositories(AppDbContext context)
        {
            this.context = context;
        }
        public async void CreateNewOrder(Order order)
        {
            if(order == null)
            {
                throw new InvalidOperationException("Переданы некорректные данные !!!");
            }
            else
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        public async void DeleteOrderById(int orderId)
        {
            Order order = await GetOrderById(orderId);

            if(order != null)
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException("Ошибка !!! Для удаление объекта передан null !!!");
            }
        }

        public Task<List<Order>> GetOrderByDate(DateOnly date)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            if (orderId != null)
            {
                return context.Orders.Single(el => el.Id == orderId);
            }
            else
            {
                throw new InvalidOperationException("Ошибка при поиске Заказа, передан 0");
            }
        }

        public Task<List<Order>> GetOrderByPriorityId(int priorityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrderByStatusId(int statusId)
        {
            throw new NotImplementedException();
        }

        public async void UpdatePerformer(Order order, int performerId)
        {
            throw new NotImplementedException();
        }

        public async void UpdatePriorityOrder(Order order, int priorityId)
        {
            throw new NotImplementedException();
        }

        public async void UpdateStatusOrder(Order order, int statusId)
        {
            throw new NotImplementedException();
        }
    }
}
