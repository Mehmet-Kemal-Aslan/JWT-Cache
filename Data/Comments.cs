using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Caching.Data
{
    public class Comments
    {
        public int Id { get; set; }
        [ForeignKey("Users")]
        public int SenderID { get; set; }
        [ForeignKey("Users")]
        public int ReceiverID { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Content { get; set; }
    }
}
