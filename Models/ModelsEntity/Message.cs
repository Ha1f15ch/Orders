using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsEntity
{
    [Table("Messages", Schema = "dbo")]
    public class Message
    {
        public Message() { }

        [Key]
        public int Id { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }

        [ForeignKey("Performer")]
        public int? PerformerId { get; set; }

        public string Content { get; set; }
        public DateTime SendAt { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
