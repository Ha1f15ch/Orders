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
        public void CreateProfileCustomer(Customer customer, int id);
        public void UpdateProfileCustomer(Customer customer);
        public void DeleteProfileCustomer(int id);// by userId
        public void DeleteProfileCustomerByCustomerId(int id);// by customerId
        public Task<Customer> GetProfileCustomer(int id);// by userId
        public Task<List<Customer>> GetAllCustomers();
        public Task<Customer>? GetProfileCustomerByCustomerId(int id);
    }
}
