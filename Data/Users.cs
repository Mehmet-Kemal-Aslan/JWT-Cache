using Microsoft.AspNetCore.Identity;

namespace JWT_Caching.Data
{
    public class AppUser : IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string UserIDLit { get; set; }
    }
}
