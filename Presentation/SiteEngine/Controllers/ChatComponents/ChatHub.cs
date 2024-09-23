using ApplicationDbContext.ContextRepositories.Services;
using ApplicationDbContext.Interfaces;
using ApplicationDbContext.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ModelsEntity;
using System.Configuration;

namespace SiteEngine.Controllers.ChatComponents
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepositories messageRepositories;
        private readonly IRedisChatService redisChatService;
        private readonly IChatRepositories chatRepositories;
        private readonly IChatService chatService;

        public ChatHub(IMessageRepositories messageRepositories, IRedisChatService redisChatService, IChatRepositories chatRepositories, IChatService chatService)
        {
            this.messageRepositories = messageRepositories;
            this.chatRepositories = chatRepositories;
            this.redisChatService = redisChatService;
            this.chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Пользователь подключен: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Пользователь отключен: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(int chatId, int userId, bool isPerformer, bool isCustomer, string messageContent)
        {
            await chatService.SendMessage(chatId, userId, isPerformer, isCustomer, messageContent);
        }

    }
}
