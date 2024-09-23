using ApplicationDbContext.Interfaces.ServicesInterfaces;
using ApplicationDbContext.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ModelsEntity;

namespace ApplicationDbContext.ContextRepositories.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<Hub> _hubContext;
        private readonly IMessageRepositories messageRepositories;
        private readonly IRedisChatService redisChatService;

        public ChatService(IHubContext<Hub> hubContext, IMessageRepositories messageRepositories, IRedisChatService redisChatService)
        {
            _hubContext = hubContext;
            this.messageRepositories = messageRepositories;
            this.redisChatService = redisChatService;
        }

        public async Task NotifyChatCreated(Chat newChat)
        {
            await _hubContext.Clients.Group($"Chat-{newChat.Id}").SendAsync("Создано обсуждение", newChat);
            await _hubContext.Clients.Group($"Chat-{newChat.Id}").SendAsync("Создано обсуждение", newChat);
        }

        public async Task JoinChatGroup(string connectionId, int chatId, int userid)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                throw new ArgumentNullException(nameof(connectionId), "Указанный connectionId не может быть null.");
            }

            var groupName = $"Chat-{chatId}";
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task SendMessage(int chatId, int user, bool isPerformer, bool isCustomer, string messageContent)
        {
            var message = CreateMessage(chatId, user, isPerformer, isCustomer, messageContent);

            await SaveMessage(chatId, message);
            await NotifyClientsInChat(messageContent);
        }

        public ModelsEntity.Message CreateMessage(int chatId, int user, bool isPerformer, bool isCustomer, string messageContent)
        {
            return new ModelsEntity.Message()
            {
                ChatId = chatId,
                CustomerId = isCustomer ? user : (int?)null,
                PerformerId = isPerformer ? user : (int?)null,
                Content = messageContent,
                SendAt = DateTime.UtcNow,
                IsRead = false,
            };
        }

        public async Task SaveMessage(int chatId, ModelsEntity.Message message)
        {
            await redisChatService.SaveMessageToCache(chatId.ToString(), message);
            await messageRepositories.CreateMessage(message);
        }

        public async Task NotifyClientsInChat(string messageContent)
        {
            await _hubContext.Clients.All.SendAsync("Новое сообщение", messageContent);
        }
    }
}
