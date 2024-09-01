using ApplicationDbContext;
using ApplicationDbContext.Interfaces;
using SiteEngine.Models.Amplua;
using Microsoft.AspNetCore.Authorization;
using ModelsEntity;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using ApplicationDbContext.ContextRepositories;
using SiteEngine.Models.Order;
using ApplicationDbContext.Interfaces.ServicesInterfaces;

namespace SiteEngine.Controllers
{
    public class CustomerBoardController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;
        private readonly IProfilePerformerRepositories profilePerformerRepositories;
        private readonly IOrderRepositories orderRepositories;
        private readonly IOrderPriorityRepositories orderPriorityRepositories;
        private readonly IOrderStatusRepositories orderStatusRepositories;
        private readonly IServiceInterfaceGetCookieData cookieDataService;
        private readonly IOrderPerformerMappingRepositories orderPerformerMappingRepositories;

        public CustomerBoardController(AppDbContext context,
                                       IProfileCustomerRepositories profileCustomerRepository,
                                       IProfilePerformerRepositories profilePerformerRepository,
                                       IOrderRepositories orderRepository,
                                       IOrderPriorityRepositories orderPriorityRepository,
                                       IOrderStatusRepositories orderStatusRepository,
                                       IServiceInterfaceGetCookieData cookieDataService,
                                       IOrderPerformerMappingRepositories orderPerformerMappingRepositories
        )
        {
            this.context = context;
            this.profileCustomerRepositories = profileCustomerRepository;
            this.profilePerformerRepositories = profilePerformerRepository;
            this.orderRepositories = orderRepository;
            this.orderPriorityRepositories = orderPriorityRepository;
            this.orderStatusRepositories = orderStatusRepository;
            this.cookieDataService = cookieDataService;
            this.orderPerformerMappingRepositories = orderPerformerMappingRepositories;
        }

        // title page customers metods 
        [Authorize, HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(cookieDataService.GetUserIdFromCookie());

            return View(myProfileCustomerModel);
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> IndexOrderListAsync()
        {

            var filterParams = new OrderFilterParams
            {
                UserId = cookieDataService.GetUserIdFromCookie(),
                IsCustomer = true // или другое значение в зависимости от логики
            };

            var getAllMyOrders = await orderRepositories.GetOrderByCustomFilter(filterParams);
            var customerProfiles = await profileCustomerRepositories.GetAllCustomers();
            var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
            var listOrderStatuses = await orderStatusRepositories.GetOrderStatusesAsync();

            return View((getAllMyOrders, customerProfiles, listOrdersPriority, listOrderStatuses));
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> IndexOrderListByFilterAsync(
            DateOnly? dateCreateStart,
            DateOnly? dateCreateEnd,
            DateOnly? dateCanceledStart,
            DateOnly? dateCanceledEnd,
            string? statusesId,
            string? prioritiesId)
        {
            var userId = cookieDataService.GetUserIdFromCookie();

            var filterParams = new OrderFilterParams
            {
                DateCreateStart = dateCreateStart,
                DateCreateEnd = dateCreateEnd,
                DateCanceledStart = dateCanceledStart,
                DateCanceledEnd = dateCanceledEnd,
                StatusId = statusesId,
                PriorityId = prioritiesId,
                UserId = userId,
                IsCustomer = true
            };



            var getAllMyOrders = await orderRepositories.GetOrderByCustomFilter(filterParams);

            var customerProfiles = await profileCustomerRepositories.GetAllCustomers();
            var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
            var listOrderStatuses = await orderStatusRepositories.GetOrderStatusesAsync();

            return View("IndexOrderList", (getAllMyOrders, customerProfiles, listOrdersPriority, listOrderStatuses));
        }

        //performer methods
        [Authorize, HttpGet]
        public async Task<IActionResult> MyProfileCustomerAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(cookieDataService.GetUserIdFromCookie());

            return View(myProfileCustomerModel);
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> EditMyProfileCustomerAsync()
        {
            var myProfileCustomerModel = await profileCustomerRepositories.GetProfileCustomer(cookieDataService.GetUserIdFromCookie());

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

                await profileCustomerRepositories.UpdateProfileCustomer(entityCustomerProfileFullViewModel);
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
        public async Task<IActionResult> CreateOrderAsync()
        {
            var userId = cookieDataService.GetUserIdFromCookie();

            var customerProfile = await profileCustomerRepositories.GetProfileCustomer(userId);
            var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();

            var model = new CreateOrderViewModel
            {
                CustomerProfile = customerProfile,
                OrderPriorities = listOrdersPriority,
                Order = new OrderForCreateViewModel()
            };

            return View("CreateOrder", model);
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = cookieDataService.GetUserIdFromCookie();

                var customerProfile = await profileCustomerRepositories.GetProfileCustomer(userId);
                var order = model.Order;

                Order orderEntity = new Order
                {
                    TitleName = order.TitleName,
                    City = customerProfile.City,
                    Adress = order.Adress,
                    Description = order.Description,
                    ActivTime = order.ActivTime,
                    CustomerId = order.CustomerId,
                    PerformerId = null,
                    OrderStatus = "N",
                    OrderPriority = order.OrderPriority,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    DeletedDate = null,
                };

                await orderRepositories.CreateNewOrder(orderEntity);
                return RedirectToAction("IndexOrderList");
            }
            else
            {
                var userId = cookieDataService.GetUserIdFromCookie();
                model.CustomerProfile = await profileCustomerRepositories.GetProfileCustomer(userId);
                model.OrderPriorities = await orderPriorityRepositories.GetOrderPrioritiesAsync();

                return View("CreateOrder", model);
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> OrderAsync(int id)
        {
            if (id <= 0)
            {
                return View("Error");
            }
            else
            {
                var userId = cookieDataService.GetUserIdFromCookie();

                var order = await orderRepositories.GetOrderById(id);
                var customerProfile = await profileCustomerRepositories.GetProfileCustomer(userId);
                var performerProfile = await profilePerformerRepositories.GetProfilePerformerByPerformerId(order.PerformerId);
                var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
                var listOrderStatus = await orderStatusRepositories.GetOrderStatusesAsync();

                var listQueuePerformerRequests = await orderPerformerMappingRepositories.GetListOrderPerformersRequests(order.Id, null);
                var listPerformers = await profilePerformerRepositories.GetPerformers();

                var model = new DetailOrderViewModelForCustomer
                {
                    Order = order,
                    CustomerProfile = customerProfile,
                    PerformerProfile = performerProfile,
                    OrderPriorities = listOrdersPriority,
                    OrderStatuses = listOrderStatus,
                    OrderPerformerMappings = listQueuePerformerRequests,
                    Performers = listPerformers
                };

                return View(model);
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> UpdateOrderAsync(int id)
        {
            if (id <= 0)
            {
                return View("Error");
            }
            else
            {
                var userId = cookieDataService.GetUserIdFromCookie();
                var customerProfile = await profileCustomerRepositories.GetProfileCustomer(userId);
                var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
                var order = await orderRepositories.GetOrderById(id);


                var model = new UpdateOrderViewModel
                {
                    CustomerProfile = customerProfile,
                    OrderPriorities = listOrdersPriority,
                    Order = new OrderForUpdateViewModel
                    {
                        Id = id,
                        TitleName = order.TitleName,
                        Adress = order.Adress,
                        Description = order.Description,
                        ActivTime = order.ActivTime,
                        CustomerId = order.CustomerId,
                        OrderPriority = order.OrderPriority
                    }
                };

                return View(model);
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> UpdateOrderAsync(int id, OrderForUpdateViewModel order)
        {
            if (order is not null)
            {
                var orderInDb = await orderRepositories.GetOrderById(id);

                Order orderForUpdate = new Order
                {
                    Id = id,
                    TitleName = order.TitleName,
                    City = orderInDb.City,
                    Adress = order.Adress,
                    Description = order.Description,
                    ActivTime = order.ActivTime,
                    CustomerId = orderInDb.CustomerId,
                    PerformerId = orderInDb.PerformerId,
                    OrderStatus = orderInDb.OrderStatus,
                    OrderPriority = order.OrderPriority,
                    CreatedDate = orderInDb.CreatedDate,
                    UpdatedDate = DateTime.Now,
                    DeletedDate = orderInDb.DeletedDate,
                };

                await orderRepositories.UpdateOrder(orderForUpdate);

                return RedirectToAction("Order", new { id = orderForUpdate.Id });
            }
            else
            {
                return View(order);
            }

        }

        [Authorize, HttpPost]
        public async Task<IActionResult> UpdateOrderPriorityAsync(int id, string orderPriority)
        {
            if (id > 0 && orderPriority is not null)
            {
                await orderRepositories.UpdatePriorityOrder(id, orderPriority);

                return RedirectToAction("Order", new { id = id });
            }
            else
            {
                return RedirectToAction("Order", new { id = id });
            }
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            if (id > 0)
            {
                await orderPerformerMappingRepositories.RemoveRequests(id);
                await orderRepositories.DeleteOrderById(id);
                
                return RedirectToAction("IndexOrderList");
            }
            else
            {
                return RedirectToAction("Order", new { id = id });
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> CancelOrderAsync(int id /*OrderId*/, int customerId)
        {
            if(id <= 0 || customerId <= 0)
            {
                return RedirectToAction("Order", new { id = id });
            }
            else
            {
                var order = await orderRepositories.GetOrderById(id);
                var customer = await profileCustomerRepositories.GetProfileCustomerByCustomerId(customerId);

                if (order is null || customer is null)
                {
                    return RedirectToAction("IndexOrderList");
                }

                //Здесь должна быть описана работа с очередь. в которой могут находиться отклиk
                return RedirectToAction("Order", new { id = id });
            }
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> ApprovePerformerForOrderAsync(int orderId, int performerId)
        {
            if (orderId <= 0 || performerId <= 0)
            {
                return RedirectToAction("Order", new { id = orderId });
            }
            else
            {
                await orderRepositories.UpdatePerformer(orderId, performerId);
                await orderPerformerMappingRepositories.RemoveRequests(orderId);

                return RedirectToAction("Order", new { id = orderId });
            }
        }
    }
}
