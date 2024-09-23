using ApplicationDbContext;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.Interfaces;
using ApplicationDbContext.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ModelsEntity;
using SiteEngine.Models.Amplua;
using System.Linq;

namespace SiteEngine.Controllers
{
    public class AmpluaController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;
        private readonly IProfilePerformerRepositories profilePerformerRepositories;
        private readonly IPerformerServiceMappingRepositories performerServiceMappingRepositories;
        private readonly IServiceRepository serviceRepository;
        private readonly IServiceInterfaceGetCookieData cookieDataService;

        public AmpluaController(IProfileCustomerRepositories profileCustomerRepositories, 
                                IProfilePerformerRepositories profilePerformerRepositories, 
                                IPerformerServiceMappingRepositories performerServiceMappingRepositories,
                                IServiceRepository serviceRepository,
                                IServiceInterfaceGetCookieData cookieDataService,
                                AppDbContext context)
        {
            this.profileCustomerRepositories = profileCustomerRepositories;
            this.profilePerformerRepositories = profilePerformerRepositories;
            this.performerServiceMappingRepositories = performerServiceMappingRepositories;
            this.serviceRepository = serviceRepository;
            this.cookieDataService = cookieDataService;
            this.context = context;
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {

            User user = await context.Users.FindAsync(cookieDataService.GetUserIdFromCookie());
            
            if (user == null)
            {
                return View();
            }

            return View(user);
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
                    UserId = cookieDataService.GetUserIdFromCookie(),
                };

                profileCustomerRepositories.CreateProfileCustomer(entityCustomer, cookieDataService.GetUserIdFromCookie());

                return RedirectToAction("MyProfileCustomer", "CustomerBoard");
            }
            else
            {
                return View("CreateProfileCustomer", customer);
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> CreateProfilePerformerAsync()
        {
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CreateProfilePerformerAsync(PerformerProfileViewModel performer)
        {
            if (ModelState.IsValid)
            {
                Performer newPerformer = new Performer
                {
                    FirstName = performer.FirstName,
                    LastName = performer.LastName,
                    MiddleName = performer.MiddleName,
                    PhoneNumber = performer.PhoneNumber,
                    City = performer.City,
                    UserId = cookieDataService.GetUserIdFromCookie(),
                    AverageRating = 0
                };

                await profilePerformerRepositories.CreateProfilePerformer(newPerformer, cookieDataService.GetUserIdFromCookie());

                return RedirectToAction("MyProfilePerformer", "PerformerBoard");
            }
            else
            {
                return View("CreateProfilePerformer", performer);
            }
        }
    }
}
