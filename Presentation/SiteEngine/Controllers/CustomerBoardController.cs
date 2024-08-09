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
        private readonly IOrderRepositories orderRepositories;
        private readonly IOrderPriorityRepositories orderPriorityRepositories;
        private readonly IOrderStatusRepositories orderStatusRepositories;
        private readonly IServiceInterfaceGetCookieData cookieDataService;

        public CustomerBoardController(AppDbContext context, 
                                       IProfileCustomerRepositories profileCustomerRepository,                            
                                       IOrderRepositories orderRepository,
                                       IOrderPriorityRepositories orderPriorityRepository,
                                       IOrderStatusRepositories orderStatusRepository,
                                       IServiceInterfaceGetCookieData cookieDataService
        )
        {
            this.context = context;
            this.profileCustomerRepositories = profileCustomerRepository;
            this.orderRepositories = orderRepository;
            this.orderPriorityRepositories = orderPriorityRepository;
            this.orderStatusRepositories = orderStatusRepository;
            this.cookieDataService = cookieDataService;
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
            var getAllMyOrders = await orderRepositories.GetOrderByCustomFilter(null, null, null, null, null, null, cookieDataService.GetUserIdFromCookie(), true, false);
            var customerProfile = await profileCustomerRepositories.GetProfileCustomer(cookieDataService.GetUserIdFromCookie());
            var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
            var listOrderStatuses = await orderStatusRepositories.GetOrderStatusesAsync();

            return View((getAllMyOrders, customerProfile, listOrdersPriority, listOrderStatuses));
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> IndexOrderListByFilterAsync(
            DateOnly? dateCreateStart,
            DateOnly? dateCreateEnd,
            DateOnly? dateCanceledStart,
            DateOnly? dateCanceledEnd,
            string? statusId,
            string? priorityId)
        {
            var userId = cookieDataService.GetUserIdFromCookie();

            var getAllMyOrders = await orderRepositories.GetOrderByCustomFilter(dateCreateStart, dateCreateEnd, dateCanceledStart, dateCanceledEnd, statusId, priorityId, userId, true, false);

            var customerProfile = await profileCustomerRepositories.GetProfileCustomer(userId);
            var listOrdersPriority = await orderPriorityRepositories.GetOrderPrioritiesAsync();
            var listOrderStatuses = await orderStatusRepositories.GetOrderStatusesAsync();

            return View("IndexOrderList", (getAllMyOrders, customerProfile, listOrdersPriority, listOrderStatuses));
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
            if(ModelState.IsValid)
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

                orderRepositories.CreateNewOrder(orderEntity);
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
            if (id == -1)
            {
                return View("Error");
            }
            else
            {
                var order = await orderRepositories.GetOrderById(id);

                return View(order);
            }
        }
    }
}
