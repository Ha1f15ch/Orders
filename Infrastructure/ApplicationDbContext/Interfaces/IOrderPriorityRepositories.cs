using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IOrderPriorityRepositories
    {
        public Task<List<OrderPriority>> GetOrderPrioritiesAsync();
        public Task<OrderPriority> GetOrderPriorityByIdAsync(string orderPriorityId);
    }
}
