using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.Interfaces
{
    public interface IChatRoomMemberRepositories
    {
        public Task CreateNewChatRoomMember(int chatRoomId, int userId, bool isPerformer, bool isCustomer);
        public Task RemoveChatRoomMembers(int chatRoomId);
        public Task<ChatRoomMembers> GetChatRoomMemberByUserId(int userId, bool isPerformer, bool isCustomer);
        public Task<List<ChatRoomMembers>> GetChatRoomMembersByChatRoomId(int chatRoomId);
        public Task<List<ChatRoomMembers>> GetChatRoomMembersAll();
    }
}
