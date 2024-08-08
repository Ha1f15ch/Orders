using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class OrderStatusRepositories : IOrderStatusRepositories
    {
        private readonly AppDbContext context;

        public OrderStatusRepositories(AppDbContext context)
        {
            this.context = context; 
        }

        public Task<OrderStatus> GetOrderStatusByIdAsync(string orderStatusId)
        {
            if(orderStatusId is not null)
            {
                return context.OrderStatuses.SingleOrDefaultAsync(el => el.Name == orderStatusId);
            }
            else
            {
                throw new ArgumentException("Для поиска переданы неверные данные, найти запись статуса не удалось !!!");
            }
        }

        public async Task<List<OrderStatus>> GetOrderStatusesAsync()
        {
            return context.OrderStatuses.ToList();
        }
    }
}
