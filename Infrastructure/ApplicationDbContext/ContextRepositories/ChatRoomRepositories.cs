using ApplicationDbContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsEntity;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDbContext.ContextRepositories
{
    public class ChatRoomRepositories : IChatRoomRepositories
    {
        private readonly AppDbContext context;

        public ChatRoomRepositories(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateNewChatRoom(int chatId)
        {
            try
            {
                var chat = await context.Chats.FindAsync(chatId);

                if(chat != null)
                {
                    var newChatRoom = new ChatRoom()
                    {
                        ChatId = chat.Id,
                        CreatedAt = DateTime.Now,
                    };

                    await context.AddAsync(newChatRoom);
                    await context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"\n chat: {chat.GetType()} \n");

                    throw new NullReferenceException($"Не найдено значение ждя одног оиз операндов: \n chat: {chat.GetType()} \n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("При выполнении запроса - Создание чата возникла ошибка - ", ex);
            }
        }

        public async Task DeleteChatRoomByChatId(int chatId)
        {
            try
            {
                var chat = await context.Chats.FindAsync(chatId);

                if (chat != null)
                {
                    var chatRoom = await context.ChatRooms.SingleOrDefaultAsync(el => el.ChatId == chat.Id);
                    if (chatRoom != null)
                    {
                        context.ChatRooms.Remove(chatRoom);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine($"\n chat: {chatRoom.GetType()} \n");
                        throw new NullReferenceException($"Не найдено значение для комнаты чата: \n chatRoom: null \n");
                    }
                }
                else
                {
                    Console.WriteLine($"\n chat: {chat.GetType()} \n");

                    throw new NullReferenceException($"Не найдено значение для одног оиз операндов: \n chat: {chat.GetType()} \n");
                }
            } catch(Exception ex)
            {
                Console.WriteLine("При выполнении запроса - Удаление комнаты чата - ", ex);
            }
        }

        public async Task<ChatRoom> GetChatRoomByChatId(int chatId)
        {
            try
            {
                var chat = await context.Chats.FindAsync(chatId);

                if(chat != null)
                {
                    return await context.ChatRooms.SingleOrDefaultAsync(el => el.ChatId == chat.Id);
                }
                else
                {
                    Console.WriteLine($"Ошибка при поиске комнаты чата, не найдена запись чата - \n chat = null");
                    throw new NullReferenceException("Ошибка при поиске комнаты чата, не найдена запись чата");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске комнаты чата - {ex.Message}");
                return new ChatRoom();
            }
        }

        public async Task<List<ChatRoom>> GetListChatRoomsAll()
        {
            try
            {
                return await context.ChatRooms.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске записей комнат чата");
                throw new NullReferenceException("Ошибка при поиске записей комнат чата");
            }
        }
    }
}
