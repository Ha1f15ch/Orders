using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class PerformerServiceMappingRepositories : IPerformerServiceMappingRepositories
    {
        private readonly AppDbContext context;

        public PerformerServiceMappingRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreatePerformerServiceMapping(int performerId, int serviceId)
        {
            if(!(performerId == -1 || serviceId == -1)) // принятые значение корректные - создаем запись
            {
                context.PerformerServicesMapping.Add(new PerformerServiceMapping
                {
                    PerformerId = performerId,
                    ServiceId = serviceId
                });
                context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeletePerformerServiceMapping(int performerId, int serviceId)
        {
            var modelForDelete = await GetModelFromPerformerServiceMappingRepositories(performerId, serviceId);
            if (modelForDelete is not null)
            {
                context.PerformerServicesMapping.Remove(modelForDelete);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeletePerformerServiceMappingByPerformerId(int performerId)
        {
            var modelsForDelete = await GetAllPerformerServiceMappingByPerformerId(performerId);
            if(modelsForDelete is not null)
            {
                foreach (var model in modelsForDelete)
                {
                    context.PerformerServicesMapping.Remove(model);
                    context.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<PerformerServiceMapping>> GetAllPerformerServiceMappingByPerformerId(int performerId)
        {
            if(performerId == 0)
            {
                throw new ArgumentException("Для поиска переданы неверные данные, найти запись не удалось");
            }
            else
            {
                var selectedData = from data in context.PerformerServicesMapping
                                   where data.PerformerId == performerId
                                   select data;

                return selectedData.ToList();
            }
        }

        public Task<List<PerformerServiceMapping>> GetAllPerformerServiceMappingByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetIdFromPerformerServiceMappingRepositories(int performerId, int serviceId)
        {
            if(performerId == 0 && serviceId == 0)
            {
                return 0;
            }
            else
            {
                return context.PerformerServicesMapping.Single(el => (el.PerformerId == performerId && el.ServiceId == serviceId)).Id;
            }
        }

        public async Task<PerformerServiceMapping> GetModelFromPerformerServiceMappingRepositories(int performerId, int serviceId)
        {
            if (performerId == 0 && serviceId == 0)
            {
                throw new ArgumentException("Для поиска переданы неверные данные, найти запись не удалось");
            }
            else
            {
                return context.PerformerServicesMapping.Single(el => (el.PerformerId == performerId && el.ServiceId == serviceId));
            }
        }
    }
}
