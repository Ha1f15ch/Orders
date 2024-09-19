using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IChatRepositories
    {
        Task<Chat> CreateChat(int orderId, int customerId, int performerId);
        Task<List<Chat>> GetChatsByUserId(int userId, bool isPerformer);
        Task<Chat> GetChatbyId(int chatId);
    }
}
