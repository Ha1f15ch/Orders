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
    public class OrderPriorityRepositories : IOrderPriorityRepositories
    {
        private readonly AppDbContext context;

        public OrderPriorityRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<OrderPriority>> GetOrderPrioritiesAsync()
        {
            return context.OrderPriority.ToList();
        }

        public Task<OrderPriority> GetOrderPriorityByIdAsync(string orderPriorityId)
        {
            if(orderPriorityId != null)
            {
                return context.OrderPriority.SingleOrDefaultAsync(el => el.Name == orderPriorityId);
            }
            else
            {
                throw new ArgumentException("Для поиска переданы неверные данные, найти запись приоритета не удалось !!!");
            }
        }
    }
}
