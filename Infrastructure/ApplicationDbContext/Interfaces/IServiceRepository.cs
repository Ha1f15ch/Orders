using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IServiceRepository
    {
        void AddNewService(Service service);
        public IEnumerable<Service> GetAllService();
        public Service GetServiceById(int id);
        public void UpdateServiceByModel(Service service);
        public void DeleteServiceById(int id);
    }
}
