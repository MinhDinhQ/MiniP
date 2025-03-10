using Microsoft.EntityFrameworkCore;
using shared.Model;


namespace API.Server;

    public class RedditDbContext : DbContext
    {
        public RedditDbContext(DbContextOptions<RedditDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
