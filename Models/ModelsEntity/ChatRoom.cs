using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("ChatRooms", Schema = "dbo")]
    public class ChatRoom
    {
        public ChatRoom() { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<ChatRoomMembers> ChatRoomMembers { get; set; }
    }
}
