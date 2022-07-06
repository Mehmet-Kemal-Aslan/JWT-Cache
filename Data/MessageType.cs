using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Caching.Data
{
    public class MessageType
    {
        public int Id { get; set; }
        [ForeignKey("Messages")]
        public int MessageId { get; set; }
        public string MessagesType { get; set; }
    }
}
