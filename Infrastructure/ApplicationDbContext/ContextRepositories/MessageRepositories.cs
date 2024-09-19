using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class MessageRepositories : IMessageRepositories
    {
        private readonly AppDbContext context;

        public MessageRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Message> CreateMessage(Message message)
        {
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }

        public async Task DeleteMessage(int messageId)
        {
            var message = await GetMessageByid(messageId);

            if(message is not null)
            {
                context.Messages.Remove(message);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Message> GetMessageByid(int messageId)
        {
            return await context.Messages.FindAsync(messageId);
        }

        public async Task<List<Message>> GetMessagesByChatId(int chatId)
        {
            return await context.Messages.Where(el => el.ChatId == chatId).ToListAsync();
        }

        public async Task UpdateMessage(Message message)
        {
            context.Messages.Update(message);
            await context.SaveChangesAsync();
        }
    }
}
