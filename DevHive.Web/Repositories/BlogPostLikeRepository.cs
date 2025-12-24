using DevHive.Web.Data;
using DevHive.Web.Models.Domain;
using DevHive.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly DevHiveDbContext devHiveDbContext;

        public BlogPostLikeRepository(DevHiveDbContext devHiveDbContext)
        {
            this.devHiveDbContext = devHiveDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await devHiveDbContext.BlogPostLike.AddAsync(blogPostLike);
            await devHiveDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await devHiveDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await devHiveDbContext.BlogPostLike
                .CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
