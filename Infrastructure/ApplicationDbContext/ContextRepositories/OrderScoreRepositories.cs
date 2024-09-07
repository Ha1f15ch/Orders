using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelsEntity;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class OrderScoreRepositories : IOrderScoreRepositories
    {
        private readonly AppDbContext context;

        public OrderScoreRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateComment(int orderId, int customerId, int performerId, int rating, string comment)
        {
            if(orderId > 0 && customerId > 0 && performerId > 0)
            {
                var order = await context.Orders.FindAsync(orderId);
                var customerProfile = await context.Customers.FindAsync(customerId);
                var performerProfile = await context.Performers.FindAsync(customerId);

                if(order is not null && customerProfile is not null && performerProfile is not null)
                {
                    var createdModel = new OrderScore
                    {
                        OrderId = order.Id,
                        CustomerId = customerProfile.Id,
                        PerformerId = performerProfile.Id,
                        Rating = rating is int ? (int)rating : 0,
                        Comment = comment,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        DeletedDate = null,
                    };

                    context.OrderScores.Add(createdModel);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException("Ошибка при выполнении поиска данных !!!");
                }
            }
            else
            {
                throw new InvalidOperationException("В параметр метода передано некорректное значение !!!");
            }
        }

        public async Task<List<OrderScore>> GetCommentsByPerformerId(int performerId)
        {
            if(performerId > 0)
            {
                try
                {
                    var profilePerformer = await context.Performers.FindAsync(performerId);

                    if (profilePerformer != null)
                    {
                        return await context.OrderScores.Where(el => el.PerformerId == profilePerformer.Id).ToListAsync();
                    }
                    else
                    {
                        throw new ArgumentNullException("Не удалось найти заказ по заданному id");
                    }
                }
                catch(Exception e)
                {
                    throw new ArgumentNullException("Ошибка при выполнении поиска отзыва !!!", e);
                }
            }
            else
            {
                throw new InvalidOperationException("В параметр метода передано некорректное значение !!!");
            }
        }

        public async Task<OrderScore> GetCommentByCommentId(int commentId)
        {
            if(commentId > 0)
            {
                try
                {
                    return await context.OrderScores.FindAsync(commentId);
                }
                catch(Exception e)
                {
                    throw new ArgumentNullException("Не удалось найти значение по ID", e);
                }
            }
            else
            {
                throw new InvalidOperationException("В параметр метода передано некорректное значение !!!");
            }
        }

        public async Task<OrderScore> GetCommentByOrderId(int orderId)
        {
            if (orderId > 0)
            {
                try
                {
                    var order = await context.Orders.FindAsync(orderId);

                    if (order != null)
                    {
                        return await context.OrderScores.SingleOrDefaultAsync(el => el.OrderId == order.Id);
                    }
                    else
                    {
                        throw new ArgumentNullException("Не удалось найти заказ по заданному id");
                    }
                    
                }
                catch (Exception e)
                {
                    throw new ArgumentNullException("Ошибка при выполнении поиска отзыва !!!", e);
                }
            }
            else
            {
                throw new InvalidOperationException("В параметр метода передано некорректное значение !!!");
            }
        }

        public async Task<double> GetNewRatingForPerformer(int performerId)
        {
            if (performerId > 0)
            {
                var performerProfile = await context.Performers.FindAsync(performerId);
                var arrRating = (await GetCommentsByPerformerId(performerId)).Select(comment => comment.Rating).ToArray();

                if (arrRating.Length > 0)
                {
                    double sum = 0;
                    foreach (var number in arrRating)
                    {
                        sum += number;
                    }

                    return (double)sum / arrRating.Length;
                }
                else
                {
                    return (int)performerProfile.AverageRating;
                }
            }
            else
            {
                throw new InvalidOperationException("В параметр метода передано некорректное значение !!!");
            }
        }

        public async Task RemoveComment(int orderId)
        {
            if (orderId > 0)
            {
                var comment = await context.OrderScores.SingleOrDefaultAsync(el => el.OrderId == orderId);

                if (comment is not null)
                {
                    var performer = await context.Performers.FindAsync(comment.PerformerId);

                    if (performer is not null)
                    {
                        context.OrderScores.Remove(comment);

                        performer.AverageRating = await GetNewRatingForPerformer(performer.Id);

                        await context.SaveChangesAsync();
                    }

                }
                else
                {
                    throw new ArgumentNullException("Ошибка при выполнении поиска отзыва и профиля исполнителя !!!");
                }
            }
            else
            {
                throw new InvalidOperationException("В параметр метода передано некорректное значение !!!");
            }
        }
    }
}
