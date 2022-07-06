using System.ComponentModel.DataAnnotations.Schema;


namespace JWT_Caching.Data
{
    public class Requests
    {
        public int Id { get; set; }
        [ForeignKey("Users")]
        public int SenderID { get; set; }
        [ForeignKey("Users")]
        public int ReceiverID { get; set; }
    }
}
