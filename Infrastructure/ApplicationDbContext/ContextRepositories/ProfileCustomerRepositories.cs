using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class ProfileCustomerRepositories : IProfileCustomerRepositories
    {
        private readonly AppDbContext context;

        public ProfileCustomerRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async void CreateProfileCustomer(Customer customer, int id)
        {
            User uItem = await FindUserById(id);

            if (uItem != null && uItem.IsCustomer != true)
            {
                context.Customers.Add(customer);
                uItem.IsCustomer = true;
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Пользователя с таким id не найдено, либо профиль заказчика уже привязан");
            }
        }

        public async void DeleteProfileCustomer(int id)//by userId
        {
            User user = await FindUserById(id);

            context.Customers.Remove(await GetProfileCustomer(id));
            user.IsCustomer = false;
            context.SaveChanges();
            //После появления заказов, нужно подумать над тем, чтобы оставить заказ с пометкой о том, что автор удалил свой лпрофиль
        }

        public async void DeleteProfileCustomerByCustomerId(int customerId)
        {
            Customer customerEntity = await GetProfileCustomerByCustomerId(customerId);
            
            if (customerEntity != null)
            {
                User user = await FindUserById(customerEntity.UserId);

                if (user != null)
                {
                    context.Customers.Remove(customerEntity);
                    user.IsCustomer = false;
                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Пользователя с таким id не найдено");
                }
            }
            else
            {
                throw new InvalidOperationException("Профиль не найден по такому id");
            }
        }

        public async Task<Customer> GetProfileCustomer(int userId)// by userId
        {
            if (userId != 0)
            {
                User uEl = await FindUserById(userId);
                if (uEl is null || uEl.IsCustomer == false)
                {
                    throw new InvalidOperationException("Пользователя с таким id не найдено, либо у него нет профиля Customer !!!");
                }
                else
                {
                    return context.Customers.Single(el => el.UserId == uEl.Id);
                }
            }
            else
            {
                throw new InvalidOperationException("Пользователя с таким id не найдено !!!");
            }

        }

        public async Task<Customer> GetProfileCustomerByCustomerId(int customerId)
        {
            if(customerId != 0)
            {
                return context.Customers.Single(el => el.Id == customerId);
            }
            else
            {
                throw new InvalidOperationException("Профиль Customer с таким id не найден !!!");
            }
        }

        public async void UpdateProfileCustomer(Customer customer)
        {
            var updatedItem = await GetProfileCustomerByCustomerId(customer.Id);

            if(updatedItem is not null)
            {
                updatedItem.LastName = customer.LastName;
                updatedItem.FirstName = customer.FirstName;
                updatedItem.MiddleName = customer.MiddleName;
                updatedItem.Email = customer.Email;
                updatedItem.PhoneNumber = customer.PhoneNumber;
                updatedItem.City = customer.City;
                updatedItem.Adress = customer.Adress;
                updatedItem.UserId = customer.UserId;
                //попробовать переписать через forEach

                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Профиля заказчика с указанным id не найдео !");
            }
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
    }
}
