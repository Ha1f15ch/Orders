using ApplicationDbContext.Interfaces.ServicesInterfaces;
using Microsoft.Extensions.Caching.Distributed;
using ModelsEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories.Services
{
    public class RedisChatService : IRedisChatService
    {
        private readonly IDistributedCache _cache;

        public RedisChatService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SaveMessageToCache(string chatId, Message message)
        {
            var messages = await _cache.GetStringAsync(chatId) ?? "[]";
            var messageList = JsonConvert.DeserializeObject<List<Message>>(messages);
            messageList.Add(message);
            await _cache.SetStringAsync(chatId, JsonConvert.SerializeObject(messageList));
        }

        public async Task<List<Message>> GetMessagesFromCache(string chatId)
        {
            var messages = await _cache.GetStringAsync(chatId);
            return messages != null ? JsonConvert.DeserializeObject<List<Message>>(messages) : new List<Message>();
        }

        public async Task DeleteMessageFromCache(string chatId)
        {
            await _cache.RemoveAsync(chatId);
        }
    }
}
