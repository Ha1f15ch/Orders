using ApplicationDbContext;
using ApplicationDbContext.ContextRepositories;
using ApplicationDbContext.ContextRepositories.Services;
using ApplicationDbContext.Interfaces;
using ApplicationDbContext.Interfaces.ServicesInterfaces;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ModelsEntity;
using SiteEngine.Controllers.ChatComponents;
using SiteEngine.Models.Chats;

namespace SiteEngine.Controllers
{
    public class PerformerChatController : BaseController
    {
        private readonly AppDbContext context;
        private readonly IChatService chatService;
        private readonly IChatRepositories chatRepositories;
        private readonly IOrderRepositories orderRepositories;
        private readonly IProfilePerformerRepositories profilePerformerRepositories;
        private readonly IProfileCustomerRepositories profileCustomerRepositories;
        private readonly IServiceInterfaceGetCookieData serviceInterfaceGetCookieData;
        private readonly IUserRepositories userRepositories;
        private readonly IMessageRepositories messageRepositories; // Пока что без использования Redis

        public PerformerChatController(AppDbContext context,
                                       IChatService chatService, 
                                       IChatRepositories chatRepositories, 
                                       IOrderRepositories orderRepositories,
                                       IProfileCustomerRepositories profileCustomerRepositories,
                                       IProfilePerformerRepositories profilePerformerRepositories,
                                       IServiceInterfaceGetCookieData serviceInterfaceGetCookieData,
                                       IUserRepositories userRepositories,
                                       IMessageRepositories messageRepositories) // Пока что без использования Redis
        {
            this.context = context;
            this.chatService = chatService;
            this.chatRepositories = chatRepositories;
            this.orderRepositories = orderRepositories;
            this.profileCustomerRepositories = profileCustomerRepositories;
            this.profilePerformerRepositories = profilePerformerRepositories;
            this.serviceInterfaceGetCookieData = serviceInterfaceGetCookieData;
            this.userRepositories = userRepositories;
            this.messageRepositories = messageRepositories; // Пока что без использования Redis
            
        }

        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var userId = serviceInterfaceGetCookieData.GetUserIdFromCookie();
                var performer = await profilePerformerRepositories.GetProfilePerformer(userId);
                var listChats = await chatRepositories.GetChatsByUserId(userId, true);
                var listOrders = await orderRepositories.GetAllOrderByPerformerId(performer.Id);
                var listCustomers = await profileCustomerRepositories.GetAllCustomers();

                if (performer is null)
                {
                    return View("Error");
                }
                else
                {
                    var model = new ListChatsForPerformer()
                    {
                        ListChats = listChats is not null ? listChats : null,
                        ListOrders = listOrders is not null ? listOrders : null,
                        PerformerProfile = performer,
                        ListPerformers = null,
                        CustomerProfile = null,
                        ListCustomers = listCustomers is not null ? listCustomers : null
                    };

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке списка чатов пользователя - ", ex.Message);
                return View();
            }
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(int chatId, int userId, string messageContent)
        {
            try
            {
                if(chatId <= 0 || userId <= 0 || messageContent.Length == 0)
                {
                    
                    await chatService.SendMessage(chatId, userId, true, false, messageContent);

                    return Ok(StatusCode(200));
                }
                else
                {
                    return BadRequest();
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Ошибка при отправке сообщения", ex);
                return BadRequest("ошибка");
            }
        }

        public class CreateChatRequest
        {
            public int UserId { get; set; }
            public int OrderId { get; set; }
            public string ConnectionId { get; set; }
        }

        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChatAsync([FromBody] CreateChatRequest request)
        {
            try
            {
                int userId = request.UserId;
                int orderId = request.OrderId;
                string connectionId = request.ConnectionId;

                Console.WriteLine($"\nПереданные данные: \nuserId = {userId},\norderId = {orderId},\nconnectionId = {connectionId}\n");
                
                if (userId > 0 && orderId > 0)
                {
                    var performerProfile = await profilePerformerRepositories.GetProfilePerformer(userId);
                    var order = await orderRepositories.GetOrderById(orderId);

                    if(performerProfile is not null && order is not null)
                    {
                        // Подготавливаем данные для передачи в модель для рендера и создания групп в SignalR
                        var newChat = await chatRepositories.CreateChat(order.Id, order.CustomerId, performerProfile.Id);
                        var customerProfile = await profileCustomerRepositories.GetProfileCustomerByCustomerId(newChat.CustomerId);
                        var messages = await messageRepositories.GetMessagesByChatId(newChat.Id);

                        // Создаем соединение и группы в SignalR, отправляем уведомления участникам чата/группы
                        await chatService.JoinChatGroup(connectionId, newChat.Id, newChat.CustomerId);
                        await chatService.JoinChatGroup(connectionId, newChat.Id, newChat.PerformerId);
                        await chatService.NotifyChatCreated(newChat);

                        // Создаем модель для рендера страницы
                        var model = new ListChatsForPerformer
                        {
                            ListChats = await chatRepositories.GetChatsByUserId(userId, true),
                            ListOrders = await orderRepositories.GetAllOrderByPerformerId(newChat.PerformerId),
                            PerformerProfile = performerProfile,
                            CurrentChat = new ChatViewModel
                            {
                                ChatId = newChat.Id,
                                ChatTitle = $"обсуждение по задаче {order.TitleName}",
                                Messages = messages,
                                User = await userRepositories.GetUserByUserId(userId),
                                Performer = performerProfile,
                                Customer = customerProfile 
                            }
                        };

                        return RedirectToAction("Index", model);
                    } 
                    else
                    {
                        return BadRequest(new { message = "Профиль исполнителя или заказ не найдены." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Некорректные идентификаторы." });
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Переданы некорректные данные", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
