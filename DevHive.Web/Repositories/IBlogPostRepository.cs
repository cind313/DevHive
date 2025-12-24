using DevHive.Web.Models.Domain;

namespace DevHive.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(
            string? searchQuery = null,
              string? sortBy = null,
            string? sortDirection = null,
            int pageSize = 100,
            int pageNumber = 1);

        Task<BlogPost?> GetAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> AddAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);

        Task<int> CountAsync();
    }
}
