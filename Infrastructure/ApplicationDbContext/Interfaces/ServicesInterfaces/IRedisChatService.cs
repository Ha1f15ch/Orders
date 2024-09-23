using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces.ServicesInterfaces
{
    public interface IRedisChatService
    {
        public Task SaveMessageToCache(string chatId, Message message);
        public Task<List<Message>> GetMessagesFromCache(string chatId);
        public Task DeleteMessageFromCache(string chatId);
    }
}
