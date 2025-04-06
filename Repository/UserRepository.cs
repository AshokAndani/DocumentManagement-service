using DocumentManagement.Entities;
using DocumentManagement.Persistance;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository
{
    /// <summary>
    /// provides methods to work on User in DB
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetUserByName(string userName);
        Task<User?> GetUserById(int Id);

        Task<Role?> GetRoleByName(string roleName);

        Task AddUser(User user);
    }

    /// <summary>
    /// provides methods to work on User in DB
    /// </summary>
    /// <param name="DbContext"></param>
    public class UserRepository(AppDbContext DbContext) : IUserRepository
    { 
        public async Task<User?> GetUserByName(string userName)
        {
            return await DbContext.Set<User>()
                .Include(x=> x.Role)
                .FirstOrDefaultAsync(x => x.Username == userName);
        }

        public async Task<User?> GetUserById(int Id)
        {
            return await DbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task AddUser(User user)
        {
            DbContext.Set<User>().Add(user);
            await DbContext.SaveChangesAsync();
        }

        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await DbContext.Set<Role>().FirstOrDefaultAsync(x => x.Name == roleName);
        }
    }
}
