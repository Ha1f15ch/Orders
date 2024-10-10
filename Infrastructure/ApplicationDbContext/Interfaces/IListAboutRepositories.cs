using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IListAboutRepositories
    {
        public Task CreateNewListAbout(string TitleId);
        public Task UpdateLisAbouttById(int id);
        public Task<ListAbout> GetListAboutById(int id);
        public Task DeleteListAboutById(int id);
        public Task GetAll();

    }
}
