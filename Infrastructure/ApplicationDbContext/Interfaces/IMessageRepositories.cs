using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IMessageRepositories
    {
        Task<Message> CreateMessage(Message message);
        Task UpdateMessage(Message message);
        Task DeleteMessage(int messageId);
        Task<Message> GetMessageByid(int messageId);
        Task<List<Message>> GetMessagesByChatId(int chatId);
    }
}
