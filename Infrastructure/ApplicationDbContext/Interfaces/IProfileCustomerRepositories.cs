using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IProfileCustomerRepositories
    {
        public Task CreateProfileCustomer(Customer customer, int id);
        public Task UpdateProfileCustomer(Customer customer);
        public Task DeleteProfileCustomer(int id);// by userId
        public Task DeleteProfileCustomerByCustomerId(int id);// by customerId
        public Task<Customer> GetProfileCustomer(int id);// by userId
        public Task<List<Customer>> GetAllCustomers();
        public Task<Customer>? GetProfileCustomerByCustomerId(int id);
    }
}
