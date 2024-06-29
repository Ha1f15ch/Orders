using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IPerformerServiceMappingRepositories
    {
        public Task<int> GetIdFromPerformerServiceMappingRepositories(int performerId, int serviceId);
        public Task<PerformerServiceMapping> GetModelFromPerformerServiceMappingRepositories(int performerId, int serviceId);
        public Task<bool> CreatePerformerServiceMapping(int performerId, int serviceId);
        public Task<bool> DeletePerformerServiceMapping(int performerId, int serviceId);
        public Task<bool> DeletePerformerServiceMappingByPerformerId(int performerId);
        public Task<List<PerformerServiceMapping>> GetAllPerformerServiceMappingByUserId(int userId);
        public Task<List<PerformerServiceMapping>> GetAllPerformerServiceMappingByPerformerId(int performerId);
    }
}
