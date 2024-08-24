using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.Interfaces.ServicesInterfaces;

namespace SiteEngine.Controllers
{
    public class PerformerBoardController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfilePerformerRepositories profilePerformerRepositories;
        private readonly IPerformerServiceMappingRepositories performerServiceMappingRepositories;
        private readonly IServiceRepository serviceRepository;
        private readonly IServiceInterfaceGetCookieData cookieDataService;
        private readonly IOrderRepositories orderRepositories;
        private readonly IOrderPriorityRepositories orderPriorityRepositories;
        private readonly IOrderStatusRepositories orderStatusRepositories;

        public PerformerBoardController(AppDbContext context, 
                                        IProfilePerformerRepositories profilePerformerRepositories, 
                                        IPerformerServiceMappingRepositories performerServiceMappingRepositories, 
                                        IServiceRepository serviceRepository,
                                        IServiceInterfaceGetCookieData cookieDataService,
                                        IOrderRepositories orderRepository,
                                        IOrderPriorityRepositories orderPriorityRepository,
                                        IOrderStatusRepositories orderStatusRepository)
        {
            this.context = context;
            this.profilePerformerRepositories = profilePerformerRepositories;
            this.performerServiceMappingRepositories = performerServiceMappingRepositories;
            this.serviceRepository = serviceRepository;
            this.cookieDataService = cookieDataService;
            this.orderRepositories = orderRepository;
            this.orderPriorityRepositories = orderPriorityRepository;
            this.orderStatusRepositories = orderStatusRepository;
        }

        // title page performers metods 
        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var myProfilePerformerModel = await profilePerformerRepositories.GetProfilePerformer(cookieDataService.GetUserIdFromCookie());

            return View(myProfilePerformerModel);
        }

        //performer methods
        [Authorize, HttpGet]
        public async Task<IActionResult> MyProfilePerformerAsync()
        {
            var myProfilePerformerModel = await profilePerformerRepositories.GetProfilePerformer(cookieDataService.GetUserIdFromCookie()); //Performer
            var servicesPerformerMapping = await performerServiceMappingRepositories.GetAllPerformerServiceMappingByPerformerId(myProfilePerformerModel.Id);
            var services = serviceRepository.GetAllServiceByList();

            return View((myProfilePerformerModel, servicesPerformerMapping, services));
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> EditMyProfilePerformerAsync()
        {
            var infoPerformer = await profilePerformerRepositories.GetProfilePerformer(cookieDataService.GetUserIdFromCookie());
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
                    UserId = cookieDataService.GetUserIdFromCookie(),
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
            if (selectedServices is null && performerId <= 0) // Получили некорректные данные
            {
                return RedirectToAction("MyProfilePerformer");
            }
            else if (selectedServices is null && performerId > 0) //все маппинги сняты, услуги удаляются
            {
                if (await performerServiceMappingRepositories.DeletePerformerServiceMappingByPerformerId(performerId))
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

        [Authorize, HttpGet]
        public async Task<IActionResult> IndexOrderListAsync()
        {
            var filterParams = new OrderFilterParams
            {
                UserId = cookieDataService.GetUserIdFromCookie(),
                IsPerformer = true
            };

            var getAllFreeOrder = await orderRepositories.GetOrderByCustomFilter(filterParams);
            var performerProfile = await profilePerformerRepositories.GetProfilePerformer(filterParams.UserId);
            var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
            var listOrderStatuses = await orderStatusRepositories.GetOrderStatusesAsync();

            return View((getAllFreeOrder, performerProfile, listOrdersPriority, listOrderStatuses));
        }
    }
}
