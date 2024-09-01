using ApplicationDbContext.Interfaces;
using Azure.Core;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class QueueOrderCancellationsRepositories : IQueueOrderCancellationsRepositories
    {
        private readonly AppDbContext context;

        public QueueOrderCancellationsRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddCancelRequest(int orderId, int? customerId, int? performerId)
        {
            if(orderId > 0)
            {
                var request = await GetCancelRequest(orderId);

                if(request is null)
                {
                    if (customerId > 0)
                    {
                        request = new QueueOrderCancellations
                        {
                            OrderId = orderId,
                            CustomerId = customerId,
                            PerformerId = null,
                            IsConfirmedByCustomer = true,
                            IsConfirmedByPerformer = false,
                        };
                    }

                    if (performerId > 0)
                    {
                        request = new QueueOrderCancellations
                        {
                            OrderId = orderId,
                            CustomerId = null,
                            PerformerId = performerId,
                            IsConfirmedByCustomer = false,
                            IsConfirmedByPerformer = true,
                        };
                    }

                    context.QueueOrderCancellations.Add(request);
                    await context.SaveChangesAsync();
                }
                else
                {
                    if(request.IsConfirmedByCustomer && request.CustomerId > 0 && performerId > 0)
                    {
                        request.PerformerId = performerId;
                        request.IsConfirmedByPerformer = true;
                        await context.SaveChangesAsync();
                    }

                    if(request.IsConfirmedByPerformer && request.PerformerId > 0 && customerId > 0)
                    {
                        request.CustomerId = customerId;
                        request.IsConfirmedByCustomer = true;
                        await context.SaveChangesAsync();
                    }
                }

                if(request.IsConfirmedByPerformer && request.IsConfirmedByCustomer)
                {
                    var order = await context.Orders.FindAsync(orderId);
                    if(order is not null)
                    {
                        order.OrderStatus = "C";
                        order.UpdatedDate = DateTime.Now;
                        context.QueueOrderCancellations.Remove(request);
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<QueueOrderCancellations> GetCancelRequest(int orderId)
        {
            return await context.QueueOrderCancellations.SingleOrDefaultAsync(el => el.OrderId == orderId);
        }
    }
}
