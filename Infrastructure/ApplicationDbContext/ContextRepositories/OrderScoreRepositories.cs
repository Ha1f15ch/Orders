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

        public Task CreateComment(int orderId, int customerId, int performerId, int rating, string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderScore>> GelCommentsByPerformerId(int performerId)
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

        public Task<int> GetNewRatingForPerformer(int performerId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveComment(int? commentId, int? orderId)
        {
            throw new NotImplementedException();
        }
    }
}
