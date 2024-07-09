using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace SiteEngine.Controllers
{
    public class CustomerBoardController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;

        public CustomerBoardController(AppDbContext context, IProfileCustomerRepositories profileCustomerRepositories)
        {
            this.context = context;
            this.profileCustomerRepositories = profileCustomerRepositories;
        }

        private int GetUserIdFromCookie()
        {
            if (Request.Cookies.TryGetValue("userID", out string userIDString))
            {
                if (int.TryParse(userIDString, out int userID))
                {
                    return userID;
                }
            }
            return 0;
        }

        // title page customers metods 
        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(GetUserIdFromCookie());

            return View(myProfileCustomerModel);
        }

        //performer methods
        [Authorize, HttpGet]
        public async Task<IActionResult> MyProfileCustomerAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(GetUserIdFromCookie());

            return View(myProfileCustomerModel);
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
            if (ModelState.IsValid)
            {
                Customer entityCustomerProfileFullViewModel = new Customer
                {
                    Id = customerProfileFullViewModel.Id,
                    FirstName = customerProfileFullViewModel.FirstName,
                    LastName = customerProfileFullViewModel.LastName,
                    MiddleName = customerProfileFullViewModel.MiddleName,
                    Email = customerProfileFullViewModel.Email,
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
    }
}
