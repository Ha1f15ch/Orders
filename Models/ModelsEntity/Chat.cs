using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("Chats", Schema = "dbo")]
    public class Chat
    {
        public Chat() { }

        [Key]
        public int Id { get; set; }
        
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        [ForeignKey("PerformerId")]
        public int PerformerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ChatRoom> ChatRooms { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
