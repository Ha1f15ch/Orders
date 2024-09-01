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
        public Task CreateProfilePerformer(Performer performer, int id);
        public Task UpdateProfilePerformer(Performer performer);
        public Task DeleteProfilePerformerByPerformerId(int performerId); // by performer Id 
        public Task<Performer> GetProfilePerformer(int userId); //by user userId
        public Task<Performer> GetProfilePerformerByPerformerId(int? performerId); // by performer Id
        public Task<List<Performer>> GetPerformers();
    }
}
