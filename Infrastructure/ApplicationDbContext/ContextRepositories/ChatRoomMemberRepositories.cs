using ApplicationDbContext.Interfaces;
using ModelsEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class ChatRoomMemberRepositories : IChatRoomMemberRepositories
    {
        private readonly AppDbContext context;

        public ChatRoomMemberRepositories(AppDbContext context) => this.context = context; 

        public async Task CreateNewChatRoomMember(int chatRoomId, int userId, bool isPerformer, bool isCustomer)
        {
            try
            {
                if(chatRoomId > 0 && userId > 0)
                {
                    var user = await context.Users.FindAsync(userId);
                    var chatRoom = await context.ChatRooms.FindAsync(chatRoomId);
                    
                    if (user != null && chatRoom != null)
                    {
                        if (isPerformer)
                        {
                            var performerProfile = await context.Performers.SingleOrDefaultAsync(el => el.UserId == user.Id);

                            if(performerProfile is not null)
                            {
                                var newChatRoomMember = new ChatRoomMembers()
                                {
                                    ChatRoomId = chatRoom.Id,
                                    UserId = user.Id,
                                    IsPerformer = isPerformer,
                                    IsCustomer = isCustomer,
                                    DateAdd = DateTime.Now
                                };

                                await context.ChatRoomMembers.AddAsync(newChatRoomMember);
                            }
                            else
                            {
                                throw new NullReferenceException("Значение performerProfile = null");
                            }
                        }

                        if (isCustomer)
                        {
                            var customerProfile = await context.Customers.SingleOrDefaultAsync(el => el.UserId == user.Id);

                            if (customerProfile is not null)
                            {
                                var newChatRoomMember = new ChatRoomMembers()
                                {
                                    ChatRoomId = chatRoom.Id,
                                    UserId = user.Id,
                                    IsPerformer = isPerformer,
                                    IsCustomer = isCustomer,
                                    DateAdd = DateTime.Now
                                };

                                await context.ChatRoomMembers.AddAsync(newChatRoomMember);
                            }
                            else
                            {
                                throw new NullReferenceException("Значение customerProfile = null");
                            }
                        }
                    }
                    else
                    {
                        throw new NullReferenceException($"Значение user или chatRuum = null: \n user - {user.GetType()}, \n chatRoom - {chatRoom.GetType()}");
                    }

                    await context.SaveChangesAsync(); //на данный момент не учитывается наличие уже созданных chatRoomMembers - записей
                }
                else
                {
                    throw new NullReferenceException($"Ошибка при создании нового участника чата, переданы некорректные данные: \n chatRoomId = {chatRoomId}, \n userId = {userId}");
                }

            } catch(Exception ex)
            {
                Console.WriteLine("При выполнении запроса - Создание участинка чата возникла ошибка - ", ex);
            }
        }

        public async Task<ChatRoomMembers> GetChatRoomMemberByUserId(int userId, bool isPerformer, bool isCustomer)
        {
            try
            {
                if(userId > 0)
                {
                    var user = await context.Users.FindAsync(userId);
                    if(user is not null)
                    {
                        return await context.ChatRoomMembers.SingleOrDefaultAsync(el => el.UserId == user.Id && el.IsPerformer == isPerformer && el.IsCustomer == isCustomer);
                    }
                    else
                    {
                        throw new NullReferenceException($"Ошибка при поиске участника чата, не найдена запись user: \n user = {user}");
                    }
                }
                else
                {
                    throw new NullReferenceException($"");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("При выполнении запроса - Получение участинка чата возникла ошибка - ", ex);
                return new ChatRoomMembers();
            }
        }

        public async Task<List<ChatRoomMembers>> GetChatRoomMembersAll()
        {
            try
            {
                return await context.ChatRoomMembers.ToListAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Невозможно вывести данные из контекста, ошибка - {ex.Message}");
                return new List<ChatRoomMembers>();
            }
        }

        public async Task<List<ChatRoomMembers>> GetChatRoomMembersByChatRoomId(int chatRoomId)
        {
            try
            {
                var chatRoom = await context.ChatRooms.FindAsync(chatRoomId);

                if(chatRoom is not null)
                {
                    return await context.ChatRoomMembers.Where(el => el.ChatRoomId == chatRoomId).ToListAsync();
                }
                else
                {
                    throw new NullReferenceException($"невозможно выполнить поиск данных, так как не существует связанного элемента, chatRoom - {chatRoom.GetType()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невозможно вывести данные из контекста, ошибка - {ex.Message}");
                return new List<ChatRoomMembers>();
            }
        }

        public async Task RemoveChatRoomMembers(int chatRoomId)
        {
            try
            {
                var chatRoom = await context.ChatRooms.FindAsync(chatRoomId);

                if (chatRoom is not null)
                {
                    var chatRoomMembers = await GetChatRoomMembersByChatRoomId(chatRoom.Id);

                    context.ChatRoomMembers.RemoveRange(chatRoomMembers);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new NullReferenceException($"невозможно выполнить удаление, так как не существует связанного элемента, chatRoom - {chatRoom.GetType()}");
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Невозможно выполнить удаление записи, {ex.Message}");
            }
        }
    }
}
