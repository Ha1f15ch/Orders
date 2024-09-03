using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IOrderPerformerMappingRepositories
    {
        public Task AddNewRequestPerformer(int orderId, int performerId);
        public Task RemoveRequests(int orderId);
        public Task RemoveRequestByPerformer(int orderId, int performerId);
        public Task<List<OrderPerformerMapping>> GetListOrderPerformersRequests(int? orderId, int? performerId);
    }
}
