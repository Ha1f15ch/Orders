using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelsEntity;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class ChatRepositories : IChatRepositories
    {
        private readonly AppDbContext context;

        public ChatRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Chat> CreateChat(int orderId, int customerId, int performerId)
        {
            try {
                var chat = new Chat
                {
                    OrderId = orderId,
                    CustomerId = customerId,
                    PerformerId = performerId,
                    CreatedAt = DateTime.Now
                };

                var oldChat = await context.Chats.Where(el => el.OrderId == orderId).ToListAsync();

                if(oldChat.ToArray().Length == 0)
                {
                    context.Chats.Add(chat);
                    await context.SaveChangesAsync();
                    return chat;
                }
                else
                {
                    return oldChat[0];
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Возникла ошибка при сохранении чата - ",e);
                await context.SaveChangesAsync();
                return null;
            }
        }

        public async Task<Chat> GetChatbyId(int chatId)
        {
            return await context.Chats.FindAsync(chatId);
        }

        public async Task<List<Chat>> GetChatsByUserId(int userId, bool isPerformer)
        {
            try
            {
                if (isPerformer)
                {
                    return await context.Chats.Where(el => el.PerformerId == userId).ToListAsync();
                }
                else
                {
                    return await context.Chats.Where(el => el.CustomerId == userId).ToListAsync();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("При поиске возникла ошибка - ", e);
                return await context.Chats.ToListAsync();
            }
        }
    }
}
