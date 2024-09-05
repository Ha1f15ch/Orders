using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IOrderScoreRepositories
    {
        public Task CreateComment(int orderId, int customerId, int performerId, int rating, string comment);
        public Task<int> GetNewRatingForPerformer(int performerId);
        public Task<OrderScore> GetCommentByCommentId(int commentId);
        public Task<OrderScore> GetCommentByOrderId(int orderId);
        public Task<List<OrderScore>> GelCommentsByPerformerId(int performerId);
        public Task RemoveComment(int? commentId, int? orderId);
    }
}
