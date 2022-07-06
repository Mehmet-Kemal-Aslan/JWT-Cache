using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Caching.Data
{
    public class MessageLog
    {
        public int Id { get; set; }
        [ForeignKey("Messages")]
        public int MessageId { get; set; }
        public string OldMessage { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
