using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelsEntity;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class OrderRepositories : IOrderRepositories
    {
        private readonly AppDbContext context;

        public OrderRepositories(AppDbContext context)
        {
            this.context = context;
        }
        public async void CreateNewOrder(Order order)
        {
            if (order == null)
            {
                throw new InvalidOperationException("Переданы некорректные данные !!!");
            }
            else
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        public async void DeleteOrderById(int orderId)
        {
            Order order = await GetOrderById(orderId);

            if (order != null)
            {
                if(order.PerformerId is null) //у заказа еще нет исполнителя 
                {
                    order.OrderStatus = "X";
                    order.DeletedDate = DateTime.Now;
                    order.UpdatedDate = DateTime.Now;
                    context.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException("Ошибка !!! У заказа есть исполнитель !!!");
                }
            }
            else
            {
                throw new ArgumentNullException("Ошибка !!! Для удаления объекта передан null !!!");
            }
        }

        public async void UpdateOrder(Order order)
        {
            if(order is not null)
            {
                var updatedModel = await GetOrderById(order.Id);

                updatedModel.TitleName = order.TitleName;
                updatedModel.City = order.City;
                updatedModel.Adress = order.Adress;
                updatedModel.Description = order.Description;
                updatedModel.ActivTime = order.ActivTime;
                updatedModel.CustomerId = order.CustomerId;
                updatedModel.PerformerId = order.PerformerId;
                updatedModel.OrderStatus = order.OrderStatus;
                updatedModel.OrderPriority = order.OrderPriority;
                updatedModel.CreatedDate = order.CreatedDate;
                updatedModel.UpdatedDate = DateTime.Now;
                updatedModel.DeletedDate = order.DeletedDate;

                context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException("Ошибка !!! Для обновления объекта передан null !!!");
            }
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            if (orderId > 0)
            {
                return context.Orders.Single(el => el.Id == orderId);
            }
            else
            {
                throw new InvalidOperationException("Ошибка при поиске Заказа, передан 0");
            }
        }

        public Task<List<Order>> GetAllMyOrders(int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllMyCompletedOrders(int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }
        public Task<List<Order>> GetAllMyStartedOrders(int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllMyCanceledOrders(int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetOrderByCustomFilter(OrderFilterParams filterParams)
        {
            Customer customer = null;
            Performer performer = null;

            if (filterParams.IsCustomer)
            {
                customer = await context.Customers.SingleOrDefaultAsync(el => el.UserId == filterParams.UserId);
            }

            if (filterParams.IsPerformer)
            {
                performer = await context.Performers.SingleOrDefaultAsync(el => el.UserId == filterParams.UserId);
            }

            IQueryable<Order> selectedOrders = context.Orders;

            if (filterParams.IsCustomer && customer != null)
            {
                selectedOrders = selectedOrders.Where(order => order.CustomerId == customer.Id);
            }
            else if (filterParams.IsPerformer && performer != null)
            {
                selectedOrders = selectedOrders.Where(order => order.PerformerId == performer.Id);
            }

            if (filterParams.DateCreateStart.HasValue && filterParams.DateCreateEnd.HasValue && (filterParams.DateCreateStart <= filterParams.DateCreateEnd))
            {
                selectedOrders = selectedOrders.Where(order => DateOnly.FromDateTime(order.CreatedDate) >= filterParams.DateCreateStart &&
                                                               DateOnly.FromDateTime(order.CreatedDate) <= filterParams.DateCreateEnd);
            }
            else if (filterParams.DateCreateStart.HasValue)
            {
                selectedOrders = selectedOrders.Where(order => DateOnly.FromDateTime(order.CreatedDate) >= filterParams.DateCreateStart);
            }
            else if (filterParams.DateCreateEnd.HasValue)
            {
                selectedOrders = selectedOrders.Where(order => DateOnly.FromDateTime(order.CreatedDate) <= filterParams.DateCreateEnd);
            }

            if (filterParams.DateCanceledStart.HasValue && filterParams.DateCanceledEnd.HasValue && (filterParams.DateCanceledStart <= filterParams.DateCanceledEnd))
            {
                selectedOrders = selectedOrders.Where(order => order.DeletedDate.HasValue &&
                                                               DateOnly.FromDateTime(order.DeletedDate.Value) >= filterParams.DateCanceledStart &&
                                                               DateOnly.FromDateTime(order.DeletedDate.Value) <= filterParams.DateCanceledEnd &&
                                                               (order.OrderStatus.Contains("C") || order.OrderStatus.Contains("X")));
            }
            else if (filterParams.DateCanceledStart.HasValue)
            {
                selectedOrders = selectedOrders.Where(order => order.DeletedDate.HasValue &&
                                                               DateOnly.FromDateTime(order.DeletedDate.Value) >= filterParams.DateCanceledStart &&
                                                               (order.OrderStatus.Contains("C") || order.OrderStatus.Contains("X")));
            }
            else if (filterParams.DateCanceledEnd.HasValue)
            {
                selectedOrders = selectedOrders.Where(order => order.DeletedDate.HasValue &&
                                                               DateOnly.FromDateTime(order.DeletedDate.Value) <= filterParams.DateCanceledEnd &&
                                                               (order.OrderStatus.Contains("C") || order.OrderStatus.Contains("X")));
            }

            if (!string.IsNullOrEmpty(filterParams.StatusId))
            {
                var statusIds = filterParams.StatusId.Split(',').ToList();
                selectedOrders = selectedOrders.Where(order => statusIds.Any(id => order.OrderStatus.Contains(id)));
            }

            if (!string.IsNullOrEmpty(filterParams.PriorityId))
            {
                var priorityIds = filterParams.PriorityId.Split(',').ToList();
                selectedOrders = selectedOrders.Where(order => priorityIds.Any(id => order.OrderPriority.Contains(id)));
            }

            return await selectedOrders.ToListAsync();
        }

        public async void UpdatePerformer(int orderId, int performerId)
        {
            throw new NotImplementedException();
        }

        public async void UpdatePriorityOrder(int id, string name)
        {
            var order = await GetOrderById(id);

            if(order is not null)
            {
                order.OrderPriority = name;
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Ошибка при изменении, не найдена запись изменения");
            }
        }

        public async void UpdateStatusOrder(int orderId, string statusId, int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }
    }
}
