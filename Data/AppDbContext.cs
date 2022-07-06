using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JWT_Caching.Data;

namespace JWT_Caching.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<MessageType> MessageType { get; set; }
        public DbSet<MessageLog> MessageLog { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Groups> Groups { get; set; }

    }
}
