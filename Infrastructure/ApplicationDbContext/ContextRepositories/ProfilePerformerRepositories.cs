using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class ProfilePerformerRepositories : IProfilePerformerRepositories
    {
        private readonly AppDbContext context;

        public ProfilePerformerRepositories(AppDbContext context)
        {
            this.context = context;
        }

        private async Task<User> FindUserById(int id)
        {
            if (id != 0)
            {
                return context.Users.Single(el => el.Id == id);
            }
            else
            {
                throw new ArgumentNullException("Error - id = null");
            }

        }

        public async void CreateProfilePerformer(Performer performer, int userId)
        {
            User user = await FindUserById(userId);

            if(user is not null && user.IsCustomer != true)
            {
                context.Performsers.Add(performer);
                user.IsCustomer = true;
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Пользователя с таким id не найдено !!!");
            }
        }

        public void DeleteProfilePerformerByPerformerId(int idperformerId)
        {
            throw new NotImplementedException();
        }

        public Task<Performer> GetProfilePerformer(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Performer> GetProfilePerformerByPerformerId(int performerId)
        {
            throw new NotImplementedException();
        }

        public void UpdateProfilePerformer(Performer performer)
        {
            throw new NotImplementedException();
        }
    }
}
