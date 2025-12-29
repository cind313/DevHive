namespace DevHive.Web.Repositories
{
    public class NullImageRepository : IImageRepository
    {
        public NullImageRepository() { 
        }

        public async Task<string?> UploadAsync(IFormFile file)
        {
            return await Task.FromResult<string?>(null);
        }
    }
}
