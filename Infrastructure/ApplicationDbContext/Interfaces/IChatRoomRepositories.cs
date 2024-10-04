using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IChatRoomRepositories
    {
        public Task CreateNewChatRoom(int chatId);
        public Task DeleteChatRoomByChatId(int chatId);
        public Task<ChatRoom> GetChatRoomByChatId(int chatId);
        public Task<List<ChatRoom>> GetListChatRoomsAll();
    }
}
