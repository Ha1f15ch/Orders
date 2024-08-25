using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
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

            if(user is not null && user.IsPerformer != true)
            {
                context.Performers.Add(performer);
                user.IsPerformer = true;
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Пользователя с таким id не найдено !!!");
            }
        }

        public async void DeleteProfilePerformerByPerformerId(int performerId)
        {
            Performer performer = await GetProfilePerformerByPerformerId(performerId);
            if (performer != null)
            {
                User user = await FindUserById(performer.UserId);

                if(user  != null && user.IsPerformer == true)
                {
                    user.IsPerformer = false;
                    context.Performers.Remove(performer);
                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Ошибка при удалении, не был найден пользователь по id профиля исполнителя");
                }
            }
            else
            {
                throw new InvalidOperationException("Ошибкак при удалении, не был найден профиль исполнителя");
            }
            
        }

        public async Task<Performer> GetProfilePerformer(int userId)
        {
            if (userId == 0)
            {
                throw new InvalidOperationException("Ошибка при поиске профиля, передан 0");
            }
            else
            {
                return context.Performers.Single(el => el.UserId == userId);
            }
        }

        public async Task<Performer> GetProfilePerformerByPerformerId(int? performerId)
        {

            if (performerId == null)
            {
                Performer performer = await context.Performers.SingleOrDefaultAsync(el => el.Id == performerId);

                if (performer != null)
                {
                    return performer;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async void UpdateProfilePerformer(Performer performer)
        {
            var updatedItem = await GetProfilePerformerByPerformerId(performer.Id);

            if (updatedItem != null) 
            {
                updatedItem.FirstName = performer.FirstName;
                updatedItem.LastName = performer.LastName;
                updatedItem.MiddleName = performer.MiddleName;
                updatedItem.Email = performer.Email;
                updatedItem.PhoneNumber = performer.PhoneNumber;
                updatedItem.City = performer.City;
                updatedItem.Experience = performer.Experience;
                updatedItem.Education = performer.Education;
                updatedItem.Description = performer.Description;
                updatedItem.AverageRating = performer.AverageRating;
                updatedItem.CreatedDate = performer.CreatedDate;
                updatedItem.UserId = performer.UserId;

                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Профиля исполнителя с указанным id не найдено !");
            }
        }
    }
}
