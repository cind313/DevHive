using Microsoft.AspNetCore.Identity;

namespace DevHive.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
