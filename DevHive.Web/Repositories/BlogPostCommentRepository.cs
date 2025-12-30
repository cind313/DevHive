using DevHive.Web.Models.Domain;
using DevHive.Web.Data;
using Microsoft.EntityFrameworkCore;



namespace DevHive.Web.Repositories
{
    public class BlogPostCommentRepository: IBlogPostCommentRepository
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
            return await devHiveDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
        }

        public async Task<BlogPostComment?> DeleteAsync(Guid id)
        {
            var existing = await devHiveDbContext.BlogPostComment.FindAsync(id);

            if (existing == null)
            {
                return null;
            }

            devHiveDbContext.BlogPostComment.Remove(existing);
            await devHiveDbContext.SaveChangesAsync();
            return existing;
        }

    }
}
