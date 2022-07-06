using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Caching.Data
{
    public class Messages
    {
        public int Id { get; set; }
        public string Content { get; set; }
        [ForeignKey("Users")]
        public int SenderID { get; set; }
        [ForeignKey("Users")]
        public int ReceiverID { get; set; }
        public DateTime Date { get; set; }= DateTime.Now;
    }
}
