using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class OrderPerformerMappingRepositories : IOrderPerformerMappingRepositories
    {
        private readonly AppDbContext context;

        public OrderPerformerMappingRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddNewRequestPerformer(int orderId, int performerId)
        {
            if(orderId <= 0 || performerId <= 0)
            {
                throw new ArgumentException("Переданы некорректные значения, orderId, performerId");
            }
            else
            {
                var listRequests = await GetListOrderPerformersRequests(orderId, performerId);
                var order = await context.Orders.Where(el => el.Id == orderId).FirstOrDefaultAsync();

                var newRequest = new OrderPerformerMapping()
                {
                    OrderId = orderId,
                    PerformerId = performerId
                };

                if (listRequests.ToArray().Length == 0)
                {
                    if(order.OrderStatus != "D")
                    {
                        order.OrderStatus = "D";
                    }

                    context.OrderPerformerMappings.Add(newRequest);
                    await context.SaveChangesAsync();
                }
                else
                {
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<OrderPerformerMapping>> GetListOrderPerformersRequests(int? orderId, int? performerId)
        {
            if (orderId != null && performerId != null)
            {
                return await context.OrderPerformerMappings.Where(el =>
                                                                        el.OrderId == orderId
                                                                        && el.PerformerId == performerId)
                                                                        .ToListAsync();
            }
            else if (orderId != null)
            {
                return await context.OrderPerformerMappings.Where(el => el.OrderId == orderId).ToListAsync();
            }
            else if(performerId != null)
            {
                return await context.OrderPerformerMappings.Where(el => el.PerformerId == performerId).ToListAsync();
            }
            else
            {
                return await context.OrderPerformerMappings.ToListAsync();
            }

        }

        public async Task RemoveRequests(int orderId) //применяется, когда утвержден исполнитель у заказа
        {
            if(orderId <= 0)
            {
                throw new ArgumentException("Переданы некорректные значения, orderId");
            }
            else
            {
                var listRequestsForRemove = await GetListOrderPerformersRequests(orderId, null);
                context.OrderPerformerMappings.RemoveRange(listRequestsForRemove);

                var order = await context.Orders.Where(el => el.Id == orderId).FirstOrDefaultAsync();
                order.OrderStatus = "S";

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveRequestByPerformer(int orderId, int performerId)
        {
            if(orderId <= 0 || performerId <= 0)
            {
                throw new ArgumentException("Переданы некорректные значения, orderId, performerId. Удаление невозможно");
            }
            else
            {
                var requestForRemove = await GetListOrderPerformersRequests(orderId, performerId);

                context.OrderPerformerMappings.RemoveRange(requestForRemove);
                
                await context.SaveChangesAsync();

                var listRequestsForOrder = await GetListOrderPerformersRequests(orderId, null);

                if (listRequestsForOrder.ToArray().Length == 0)
                {
                    var order = await context.Orders.Where(el => el.Id == orderId).FirstOrDefaultAsync();
                    order.OrderStatus = "N";
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}
