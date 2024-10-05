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
        public Task CreateNewListAbout(ListAbout listAbout);
        public Task UpdateLisAbouttById(ListAbout listAbout);
        public Task GetListAboutById(string id);
        public Task DeleteListAboutById(int id);
    }
}
