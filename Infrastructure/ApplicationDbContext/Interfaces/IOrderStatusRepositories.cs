using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IOrderStatusRepositories
    {
        public Task<List<OrderStatus>> GetOrderStatusesAsync();
        public Task<OrderStatus> GetOrderStatusByIdAsync(string orderStatusId);
    }
}
