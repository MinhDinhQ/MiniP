
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using shared.Model;

namespace API.Server
{
    public class RedditDbContext : DbContext
    {
        public RedditDbContext(DbContextOptions<RedditDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

    public class RedditDbContextFactory : IDesignTimeDbContextFactory<RedditDbContext>
    {
        public RedditDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RedditDbContext>();

           
            optionsBuilder.UseSqlite("Data Source=reddit.db");  

            return new RedditDbContext(optionsBuilder.Options);
        }
    }
}
