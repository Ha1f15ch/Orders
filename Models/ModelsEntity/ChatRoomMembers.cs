using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("ChatRoomMembers", Schema = "dbo")]
    public class ChatRoomMembers
    {
        public ChatRoomMembers() { }

        [Key]
        public int Id { get; set; }
        [ForeignKey("ChatRoom")]
        public int ChatRoomId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public bool IsPerformer { get; set; }
        public bool IsCustomer { get; set; }
        public DateTime DateAdd { get; set; }

        public virtual ChatRoom? ChatRoom { get; set; }
        public virtual User User { get; set; }
    }
}
