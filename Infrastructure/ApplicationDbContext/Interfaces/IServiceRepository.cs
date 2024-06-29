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
        public void AddNewService(Service service);
        public IEnumerable<Service> GetAllService();
        public List<Service> GetAllServiceByList();
        public Service GetServiceById(int id);
        public void UpdateServiceByModel(Service service);
        public void DeleteServiceById(int id);
    }
}
