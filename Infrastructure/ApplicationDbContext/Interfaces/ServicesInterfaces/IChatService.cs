using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces.ServicesInterfaces
{
    public interface IChatService
    {
        public Task NotifyChatCreated(Chat newChat);
        public Task JoinChatGroup(string connectionId, int chatId, int userid);
        public Task SendMessage(int chatId, int user, bool isPerformer, bool isCustomer, string messageContent);
        public ModelsEntity.Message CreateMessage(int chatId, int user, bool isPerformer, bool isCustomer, string messageContent);
        public Task SaveMessage(int chatId, ModelsEntity.Message message);
        public Task NotifyClientsInChat(string messageContent);
    }
}
