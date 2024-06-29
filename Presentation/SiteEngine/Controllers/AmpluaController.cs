using ApplicationDbContext;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.Interfaces;
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

        public AmpluaController(IProfileCustomerRepositories profileCustomerRepositories, 
                                IProfilePerformerRepositories profilePerformerRepositories, 
                                IPerformerServiceMappingRepositories performerServiceMappingRepositories,
                                IServiceRepository serviceRepository,
                                AppDbContext context)
        {
            this.profileCustomerRepositories = profileCustomerRepositories;
            this.profilePerformerRepositories = profilePerformerRepositories;
            this.performerServiceMappingRepositories = performerServiceMappingRepositories;
            this.serviceRepository = serviceRepository;
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
                    UserId = GetUserIdFromCookie(),
                    AverageRating = 0
                };

                profilePerformerRepositories.CreateProfilePerformer(newPerformer, GetUserIdFromCookie());

                return RedirectToAction("MyProfilePerformer");
            }
            else
            {
                return View("CreateProfilePerformer", performer);
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> MyProfilePerformerAsync()
        {
            var myProfilePerformerModel = await profilePerformerRepositories.GetProfilePerformer(GetUserIdFromCookie()); //Performer
            var servicesPerformerMapping = await performerServiceMappingRepositories.GetAllPerformerServiceMappingByPerformerId(myProfilePerformerModel.Id);
            var services = serviceRepository.GetAllServiceByList();

            return View((myProfilePerformerModel, servicesPerformerMapping, services));
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> EditMyProfilePerformerAsync()
        {
            var infoPerformer = await profilePerformerRepositories.GetProfilePerformer(GetUserIdFromCookie());
            var infoListService = serviceRepository.GetAllServiceByList();
            var infoPerformerServiceMapping = await performerServiceMappingRepositories.GetAllPerformerServiceMappingByPerformerId(infoPerformer.Id);

            if (infoPerformer == null)
            {
                return View("MyProfilePerformer");
            }
            else
            {
                PerformerProfileFullViewModel tempInfoPerformer = new PerformerProfileFullViewModel
                {
                    Id = infoPerformer.Id,
                    FirstName = infoPerformer.FirstName,
                    LastName = infoPerformer.LastName,
                    MiddleName = infoPerformer.MiddleName,
                    Email = infoPerformer.Email,
                    PhoneNumber = infoPerformer.PhoneNumber,
                    City = infoPerformer.City,
                    Experience = infoPerformer.Experience,
                    Education = infoPerformer.Education,
                    Description = infoPerformer.Description,
                    AverageRating = infoPerformer.AverageRating,
                    UserId = GetUserIdFromCookie(),
                };

                return View(tempInfoPerformer);
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> EditMyProfilePerformerAsync(PerformerProfileFullViewModel performerProfileFullViewModel)
        {
            if (ModelState.IsValid)
            {
                var performerModelFromTabble = await profilePerformerRepositories.GetProfilePerformerByPerformerId(performerProfileFullViewModel.Id);

                Performer performermodel = new Performer
                {
                    Id = performerProfileFullViewModel.Id,
                    FirstName = performerProfileFullViewModel.FirstName,
                    LastName = performerProfileFullViewModel.LastName,
                    MiddleName = performerProfileFullViewModel.MiddleName,
                    Email = performerProfileFullViewModel.Email,
                    PhoneNumber = performerProfileFullViewModel.PhoneNumber,
                    City = performerProfileFullViewModel.City,
                    Experience = performerProfileFullViewModel.Experience,
                    Education = performerProfileFullViewModel.Education,
                    Description = performerProfileFullViewModel.Description,
                    AverageRating = performerModelFromTabble.AverageRating,
                    CreatedDate = performerModelFromTabble.CreatedDate,
                    UserId = performerModelFromTabble.UserId,
                };

                profilePerformerRepositories.UpdateProfilePerformer(performermodel);
                return RedirectToAction("MyProfilePerformer");
            }
            else
            {
                return View(performerProfileFullViewModel);
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CRUDServicePerformerMappingAsync(String selectedServices, int performerId)
        {
            if(selectedServices is null && performerId == -1) // Получили некорректные данные
            {
                return RedirectToAction("MyProfilePerformer");
            }
            else if(selectedServices is null && performerId != -1) //все маппинги сняты, услуги удаляются
            {
                if(await performerServiceMappingRepositories.DeletePerformerServiceMappingByPerformerId(performerId))
                {
                    return RedirectToAction("MyProfilePerformer"); // Удаление выполнено успешно
                }
                else
                {
                    return RedirectToAction("MyProfilePerformer"); //Здесь при обмновлении ничего не выполнилось
                }
            }
            else // Указан id исполнителя и id услуг
            {
                string[] itemsServiceId = selectedServices.Split(',');

                List<int> serviceIds = new List<int>();//список id услуг, переданных с клиента

                foreach (var serviceId in itemsServiceId)
                {
                    if (int.TryParse(serviceId, out int id))
                    {
                        serviceIds.Add(id);
                    }
                }

                var listMappingServices = await performerServiceMappingRepositories.GetAllPerformerServiceMappingByPerformerId(performerId); //по id исполнителя все маппинг услуги

                var idsToAdd = serviceIds.Except(listMappingServices.Select(item => item.ServiceId)); // услуги у которых нет маппинга (создаем связи)
                foreach (var idToAdd in idsToAdd)
                {
                    await performerServiceMappingRepositories.CreatePerformerServiceMapping(performerId, idToAdd);
                }

                var idsToRemove = listMappingServices.Select(item => item.ServiceId).Except(serviceIds); //услуги, id которых не передали (удаляем записи)
                foreach (var idToRemove in idsToRemove)
                {
                    await performerServiceMappingRepositories.DeletePerformerServiceMapping(performerId, idToRemove);
                }

                return RedirectToAction("MyProfilePerformer");
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> RemoveProfilePerformer(int id)
        {
            if (id != 0)
            {
                profilePerformerRepositories.DeleteProfilePerformerByPerformerId(id);
                return RedirectToAction("Index");
            }
            else
            {
                return View("MyProfilePerformer");
            }
        }
    }
}
