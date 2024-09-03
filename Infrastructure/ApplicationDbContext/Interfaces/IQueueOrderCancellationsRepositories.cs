using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IQueueOrderCancellationsRepositories
    {
        public Task AddCancelRequest(int orderId, int? customerId, int? performerId);
        public Task DeleteCancelRequest(int orderId, int? customerId, int? performerId);
        public Task<QueueOrderCancellations> GetCancelRequest(int orderId);
    }
}