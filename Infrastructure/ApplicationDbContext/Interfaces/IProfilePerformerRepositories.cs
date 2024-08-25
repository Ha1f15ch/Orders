using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IProfilePerformerRepositories
    {
        public void CreateProfilePerformer(Performer performer, int id);
        public void UpdateProfilePerformer(Performer performer);
        public void DeleteProfilePerformerByPerformerId(int performerId); // by performer Id 
        public Task<Performer> GetProfilePerformer(int userId); //by user userId
        public Task<Performer> GetProfilePerformerByPerformerId(int? performerId); // by performer Id
    }
}
