using ModelsEntity;

namespace SiteEngine.Models.Chats
{
    public class ChatViewModel
    {
        public int ChatId { get; set; } // Идентификатор чата
        public string ChatTitle { get; set; } // Название чата
        public List<Message> Messages { get; set; } // Список сообщений
        public User User { get; set; } // User профиль исполнителя
        public Customer? Customer { get; set; }
        public Performer? Performer { get; set; }
    }
}
