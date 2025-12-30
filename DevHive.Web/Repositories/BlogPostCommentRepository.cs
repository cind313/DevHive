using DevHive.Web.Data;
using DevHive.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly DevHiveDbContext devHiveDbContext;

        public BlogPostCommentRepository(DevHiveDbContext devHiveDbContext)
        {
            this.devHiveDbContext = devHiveDbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await devHiveDbContext.BlogPostComment.AddAsync(blogPostComment);
            await devHiveDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await devHiveDbContext.BlogPostComment
                .Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
        }

        public async Task<BlogPostComment?> GetAsync(Guid id)
        {
            return await devHiveDbContext.BlogPostComment.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await devHiveDbContext.BlogPostComment.FindAsync(id);
            if (comment != null)
            {
                devHiveDbContext.BlogPostComment.Remove(comment);
                await devHiveDbContext.SaveChangesAsync();
            }
        }
    }
}
