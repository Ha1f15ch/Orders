using ApplicationDbContext.Interfaces;
using Microsoft.IdentityModel.Tokens;
using ModelsEntity;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async void DeleteOrderById(int orderId, int userId, bool isCustomer, bool isPerformer)
        {
            Order order = await GetOrderById(orderId);
            Customer customer = context.Customers.Single(el => el.UserId == userId);

            if (order != null)
            {
                if(customer is not null && order.PerformerId is null) //у заказа еще нет исполнителя 
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

        public async Task<Order> GetOrderById(int orderId)
        {
            if (orderId != -1 && orderId != 0)
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

        public async Task<List<Order>> GetOrderByCustomFilter(DateOnly? dateCreateStart = null, DateOnly? dateCreateEnd = null, DateOnly? dateCancaledStart = null, DateOnly? dateCanceledEnd = null, string? statusId = null, string? priorityId = null, int userId = 0, bool isCustomer = false, bool isPerformer = false)
         {
            var customer = isCustomer ? context.Customers.SingleOrDefault(el => el.UserId == userId) : null;
            var performer = isPerformer ? context.Performers.SingleOrDefault(el => el.UserId == userId) : null;

            if (isCustomer && !isPerformer && customer != null) //Заказчик
            {
                IQueryable<Order> selectedOrders;

                if (dateCreateStart.HasValue && dateCreateEnd.HasValue && (dateCreateStart <= dateCreateEnd))
                {
                    selectedOrders = from order in context.Orders
                                     where order.CustomerId == customer.Id &&
                                         DateOnly.FromDateTime(order.CreatedDate) >= dateCreateStart &&
                                         DateOnly.FromDateTime(order.CreatedDate) <= dateCreateEnd
                                     //Возможно имеет смысл добавить фильтр - "Показывать удаленные"
                                     select order;
                }
                else if(dateCreateStart.HasValue)
                {
                    selectedOrders = from order in context.Orders
                                     where order.CustomerId == customer.Id &&
                                         DateOnly.FromDateTime(order.CreatedDate) >= dateCreateStart
                                     select order;
                }
                else if (dateCreateEnd.HasValue)
                {
                    selectedOrders = from order in context.Orders
                                     where order.CustomerId == customer.Id &&
                                         DateOnly.FromDateTime(order.CreatedDate) <= dateCreateEnd
                                     select order;
                }
                else
                {
                    selectedOrders = from order in context.Orders select order;
                }
                
                if(dateCancaledStart.HasValue && dateCanceledEnd.HasValue && (dateCancaledStart <= dateCanceledEnd))
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.CustomerId == customer.Id &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) >= dateCancaledStart &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) <= dateCanceledEnd &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP") ||
                                         orders.OrderStatus.Contains("X"))
                                     select orders;
                }
                else if (dateCancaledStart.HasValue)
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.CustomerId == customer.Id &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) >= dateCancaledStart &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP") ||
                                         orders.OrderStatus.Contains("X"))
                                     select orders;
                }
                else if (dateCanceledEnd.HasValue)
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.CustomerId == customer.Id &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) <= dateCanceledEnd &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP") ||
                                         orders.OrderStatus.Contains("X"))
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                if(!statusId.IsNullOrEmpty())
                { //Нужно исправить, принимаемое знакчение может быть множественным
                    selectedOrders = from orders in selectedOrders
                                     where orders.CustomerId == customer.Id &&
                                           orders.OrderStatus == statusId
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                if(!priorityId.IsNullOrEmpty())
                { //Нужно исправить, принимаемое знакчение может быть множественным
                    selectedOrders = from orders in selectedOrders
                                     where orders.CustomerId == customer.Id &&
                                           orders.OrderPriority == priorityId
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                return selectedOrders.ToList();
            }
            else if(!isCustomer && isPerformer && performer != null) //Исполнитель
            {
                IQueryable<Order> selectedOrders;

                if (dateCreateStart.HasValue && dateCreateEnd.HasValue && (dateCreateStart <= dateCreateEnd))
                {
                    selectedOrders = from order in context.Orders
                                     where order.PerformerId == performer.Id &&
                                         DateOnly.FromDateTime(order.CreatedDate) >= dateCreateStart &&
                                         DateOnly.FromDateTime(order.CreatedDate) <= dateCreateEnd
                                         //Возможно имеет смысл добавить фильтр - "Показывать удаленные"
                                     select order;
                }
                else if (dateCreateStart.HasValue)
                {
                    selectedOrders = from order in context.Orders
                                     where order.PerformerId == performer.Id &&
                                         DateOnly.FromDateTime(order.CreatedDate) >= dateCreateStart
                                     select order;
                }
                else if (dateCreateEnd.HasValue)
                {
                    selectedOrders = from order in context.Orders
                                     where order.PerformerId == performer.Id &&
                                         DateOnly.FromDateTime(order.CreatedDate) <= dateCreateEnd
                                     select order;
                }
                else
                {
                    selectedOrders = from order in context.Orders select order;
                }

                if (dateCancaledStart.HasValue && dateCanceledEnd.HasValue && (dateCancaledStart <= dateCanceledEnd))
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.PerformerId == performer.Id &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) >= dateCancaledStart &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) <= dateCanceledEnd &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP"))
                                     select orders;
                }
                else if (dateCancaledStart.HasValue)
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.PerformerId == performer.Id &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) >= dateCancaledStart &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP"))
                                     select orders;
                }
                else if (dateCanceledEnd.HasValue)
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.PerformerId == performer.Id &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) <= dateCanceledEnd &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP"))
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                if (!statusId.IsNullOrEmpty())
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.PerformerId == performer.Id &&
                                           orders.OrderStatus == statusId
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                if (!priorityId.IsNullOrEmpty())
                {
                    selectedOrders = from orders in selectedOrders
                                     where orders.PerformerId == performer.Id &&
                                           orders.OrderPriority == priorityId &&
                                           orders.OrderStatus != "X"
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                return selectedOrders.ToList();
            }
            else //Любой другой (администратор)
            {
                IQueryable<Order> selectedOrders;

                if (dateCreateStart.HasValue && dateCreateEnd.HasValue && (dateCreateStart <= dateCreateEnd))
                {
                    selectedOrders = from order in context.Orders
                                     where 
                                         DateOnly.FromDateTime(order.CreatedDate) >= dateCreateStart &&
                                         DateOnly.FromDateTime(order.CreatedDate) <= dateCreateEnd
                                     //Возможно имеет смысл добавить фильтр - "Показывать удаленные"
                                     select order;
                }
                else if (dateCreateStart.HasValue)
                {
                    selectedOrders = from order in context.Orders
                                     where 
                                         DateOnly.FromDateTime(order.CreatedDate) >= dateCreateStart
                                     select order;
                }
                else
                {
                    selectedOrders = from order in context.Orders select order;
                }

                if (dateCancaledStart.HasValue && dateCanceledEnd.HasValue && (dateCancaledStart <= dateCanceledEnd))
                {
                    selectedOrders = from orders in selectedOrders
                                     where 
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) >= dateCancaledStart &&
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) <= dateCanceledEnd &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP") ||
                                         orders.OrderStatus.Contains("X"))
                                     select orders;
                }
                else if (dateCancaledStart.HasValue)
                {
                    selectedOrders = from orders in selectedOrders
                                     where 
                                         DateOnly.FromDateTime((DateTime)orders.DeletedDate) >= dateCancaledStart &&
                                         (orders.OrderStatus.Contains("CC") ||
                                         orders.OrderStatus.Contains("CP") ||
                                         orders.OrderStatus.Contains("X"))
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                if (!statusId.IsNullOrEmpty())
                {
                    selectedOrders = from orders in selectedOrders
                                     where 
                                           orders.OrderStatus == statusId
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                if (!priorityId.IsNullOrEmpty())
                {
                    selectedOrders = from orders in selectedOrders
                                     where 
                                           orders.OrderPriority == priorityId
                                     select orders;
                }
                else
                {
                    selectedOrders = from orders in selectedOrders select orders;
                }

                return selectedOrders.ToList();
            }
        }

        public async void UpdatePerformer(int orderId, int performerId)
        {
            throw new NotImplementedException();
        }

        public async void UpdatePriorityOrder(int orderId, string priorityId, int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }

        public async void UpdateStatusOrder(int orderId, string statusId, int userId, bool isCustomer, bool isPerformer)
        {
            throw new NotImplementedException();
        }
    }
}
