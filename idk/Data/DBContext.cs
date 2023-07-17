using idk.Models;
using Microsoft.EntityFrameworkCore;

namespace idk.Data
{
    public class DBContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserPic> UserPic { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
    }
}
