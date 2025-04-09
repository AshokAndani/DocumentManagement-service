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
        /// <summary>
        /// returns user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User?> GetUserByName(string userName);

        /// <summary>
        /// returns user by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<User?> GetUserById(int Id);

        /// <summary>
        /// returns role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<Role?> GetRoleByName(string roleName);

        /// <summary>
        /// adds user to DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task AddUser(User user);
    }

    /// <summary>
    /// provides methods to work on User in DB
    /// </summary>
    /// <param name="DbContext"></param>
    public class UserRepository(AppDbContext DbContext) : IUserRepository
    {
        /// <summary>
        /// returns user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByName(string userName)
        {
            return await DbContext.Set<User>()
                .Include(x=> x.Role)
                .FirstOrDefaultAsync(x => x.Username == userName);
        }

        /// <summary>
        /// returns user by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User?> GetUserById(int Id)
        {
            return await DbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == Id);
        }

        /// <summary>
        /// returns role by name
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUser(User user)
        {
            DbContext.Set<User>().Add(user);
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// returns role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await DbContext.Set<Role>().FirstOrDefaultAsync(x => x.Name == roleName);
        }
    }
}
