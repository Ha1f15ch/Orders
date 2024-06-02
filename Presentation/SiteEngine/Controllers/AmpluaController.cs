using ApplicationDbContext;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsEntity;
using SiteEngine.Models.Amplua;

namespace SiteEngine.Controllers
{
    public class AmpluaController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;

        public AmpluaController(IProfileCustomerRepositories profileCustomerRepositories, AppDbContext context)
        {
            this.profileCustomerRepositories = profileCustomerRepositories;
            this.context = context;
        }

        private int GetUserIdFromCookie()
        {
            if(Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if(int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }

            return 0;
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {

            User user = await this.context.Users.FindAsync(GetUserIdFromCookie());
            
            if (user == null)
            {
                return View();
            }

            return View(user);
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> MyProfileCustomerAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(GetUserIdFromCookie());
       
            return View(myProfileCustomerModel);
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> CreateProfileCustomerAsync()
        {
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CreateProfileCustomerAsync(CustomerProfileViewModel customer)
        {
            if (ModelState.IsValid)
            {
                Customer entityCustomer = new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    MiddleName = customer.MiddleName,
                    PhoneNumber = customer.PhoneNumber,
                    City = customer.City,
                    Adress = customer.Adress,
                    UserId = GetUserIdFromCookie(),
                };

                profileCustomerRepositories.CreateProfileCustomer(entityCustomer, GetUserIdFromCookie());

                return RedirectToAction("MyProfileCustomer");
            }
            else
            {
                return View("CreateProfileCustomer", customer);
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> EditMyProfileCustomerAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(GetUserIdFromCookie());

            CustomerProfileFullViewModel customerProfileFullViewModel = new CustomerProfileFullViewModel
            {
                Id = myProfileCustomerModel.Id,
                FirstName = myProfileCustomerModel.FirstName,
                LastName = myProfileCustomerModel.LastName,
                MiddleName = myProfileCustomerModel.MiddleName,
                Email = myProfileCustomerModel?.Email,
                PhoneNumber = myProfileCustomerModel.PhoneNumber,
                City = myProfileCustomerModel.City,
                Adress = myProfileCustomerModel.Adress,
                UserId = myProfileCustomerModel.UserId,
            };

            return View(customerProfileFullViewModel);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> EditMyProfileCustomerAsync(CustomerProfileFullViewModel customerProfileFullViewModel)
        {
            if(ModelState.IsValid)
            {
                Customer entityCustomerProfileFullViewModel = new Customer
                {
                    Id = customerProfileFullViewModel.Id,
                    FirstName = customerProfileFullViewModel.FirstName,
                    LastName = customerProfileFullViewModel.LastName,
                    MiddleName = customerProfileFullViewModel.MiddleName,
                    Email= customerProfileFullViewModel.Email,
                    PhoneNumber = customerProfileFullViewModel.PhoneNumber,
                    City = customerProfileFullViewModel.City,
                    Adress = customerProfileFullViewModel.Adress,
                    UserId = customerProfileFullViewModel.UserId,
                }; //переписать на перебор циклом

                profileCustomerRepositories.UpdateProfileCustomer(entityCustomerProfileFullViewModel);
                return RedirectToAction("MyProfileCustomer");
            }

            return View(customerProfileFullViewModel);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> RemoveProfileCustomer(int id)
        {
            if (id != 0)
            {
                profileCustomerRepositories.DeleteProfileCustomerByCustomerId(id);
                return RedirectToAction("Index");
            }
            else
            {
                return View("MyProfileCustomer");
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> CreateProfilePerformerAsync()
        {
            return View();
        }


    }
}
