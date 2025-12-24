using DevHive.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Web.Data
{
    public class DevHiveDbContext : DbContext
    {
        public DevHiveDbContext(DbContextOptions<DevHiveDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }


    }
}
