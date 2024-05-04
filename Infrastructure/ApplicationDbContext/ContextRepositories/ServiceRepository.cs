using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext context;

        public ServiceRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void AddNewService(Service service)//нужно учесть какие поля должны обрабатываться, не принимать null
        {
            context.Services.Add(service);
            context.SaveChanges();
        }

        public IEnumerable<Service> GetAllService()
        {
            return context.Services.ToList();
        }

        public Service GetServiceById(int id)
        {
            return context.Services.Single(item => item.Id == id);
        }

        public void UpdateServiceByModel(Service service)
        {
            var updatedItem = context.Services.Single(item => item.Id == service.Id);

            if (updatedItem != null)
            {
                //context.Services.Update(service);
                updatedItem.NameOfService = service.NameOfService;
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Услуга с указанным id не найдена");
            }
        }

        public void DeleteServiceById(int id)
        {
            var serviceForDelete = GetServiceById(id);

            if(serviceForDelete is not null)
            {
                var serviceCategoryMapping = context.ServiceCategoryMappings.Where(item => item.ServiceId == id);

                context.ServiceCategoryMappings.RemoveRange(serviceCategoryMapping);

                context.Services.Remove(serviceForDelete);
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Услуга с указанным id не найдена");
            }
        }
    }
}
