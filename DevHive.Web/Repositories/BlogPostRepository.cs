using DevHive.Web.Data;
using DevHive.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly DevHiveDbContext devHiveDbContext;

        public BlogPostRepository(DevHiveDbContext devHiveDbContext)
        {
            this.devHiveDbContext = devHiveDbContext;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(
            string? searchQuery,
              string? sortBy,
            string? sortDirection,
            int pageSize = 100,
            int pageNumber = 1)
        {
            var query = devHiveDbContext.BlogPosts.Include(x => x.Tags).AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x =>
                                    x.PageTitle.Contains(searchQuery) ||
                                    x.Heading.Contains(searchQuery));  
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "PublishedDate", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.PublishedDate) : query.OrderBy(x => x.PublishedDate);
                }
                 
            }

            // Pagination
            // Skip 0 Take 5 -> Page 1 of 5 results
            // Skip 5 Take next 5 -> Page 2 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);


            return await query.ToListAsync();
            //return await devHiveDbContext.Tags.ToListAsync();

            // return await devHiveDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await devHiveDbContext.AddAsync(blogPost);
            await devHiveDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await devHiveDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                devHiveDbContext.BlogPosts.Remove(existingBlog);
                await devHiveDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }

       
        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await devHiveDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await devHiveDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await devHiveDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await devHiveDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }

        public async Task<int> CountAsync()
        {
            return await devHiveDbContext.BlogPosts.CountAsync();
        }
    }
}
