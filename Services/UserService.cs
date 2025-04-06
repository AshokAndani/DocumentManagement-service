using DocumentManagement.Entities;
using DocumentManagement.Repository;

namespace DocumentManagement.Services
{
    /// <summary>
    /// Provides methods for Additional processing for Users
    /// </summary>
    public interface IUserService
    {
        Task<User?> GetByUserName(string userName);
        Task AddUser(User user);
        Task<Role?> GetRoleByName(string roleName);
    }

    /// <summary>
    /// Provides methods for Additional processing for Users
    /// </summary>
    /// <param name="userRepository"></param>
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<User?> GetByUserName(string userName)
        {
            return await userRepository.GetUserByName(userName);
        }

        public async Task AddUser(User user)
        {
            await userRepository.AddUser(user);
        }

        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await userRepository.GetRoleByName(roleName);
        }
    }
}
