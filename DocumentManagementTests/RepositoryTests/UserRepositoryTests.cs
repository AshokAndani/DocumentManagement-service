using DocumentManagement.Entities;
using DocumentManagement.Persistance;
using DocumentManagement.Repository;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagementTests.RepositoryTests
{
    public class UserRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);
        }
        [Fact]
        public async Task AddUser_ShouldAddUserToDatabase()
        {
            var user = new User
            {
                Username = "testuser",
                PasswordHash = "hashed",
                Email = "test@example.com",
                RoleId = 1,
                CreateTimestamp = DateTime.UtcNow,
                ModifyTimestamp = DateTime.UtcNow
            };

            await _repository.AddUser(user);

            var added = await _context.Set<User>().FirstOrDefaultAsync(x => x.Username == "testuser");
            Assert.NotNull(added);
        }

        [Fact]
        public async Task GetUserByName_ReturnsUser_WhenExists()
        {
            var role = new Role { Name = "Admin" };
            _context.Set<Role>().Add(role);
            var user = new User
            {
                Username = "admin",
                PasswordHash = "pass",
                Role = role,
                Email = "admin@example.com",
                CreateTimestamp = DateTime.UtcNow,
                ModifyTimestamp = DateTime.UtcNow
            };
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetUserByName("admin");
            Assert.NotNull(result);
            Assert.Equal("Admin", result?.Role?.Name);
        }

        [Fact]
        public async Task GetUserByName_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetUserByName("nouser");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsUser_WhenExists()
        {
            var user = new User { Username = "user1", Email = "u1@mail.com", RoleId = 1, PasswordHash = "jabdjb" };
            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetUserById(user.Id);
            Assert.NotNull(result);
            Assert.Equal("user1", result?.Username);
        }

        [Fact]
        public async Task GetUserById_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetUserById(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRoleByName_ReturnsRole_WhenExists()
        {
            _context.Set<Role>().Add(new Role { Name = "User" });
            await _context.SaveChangesAsync();

            var result = await _repository.GetRoleByName("User");
            Assert.NotNull(result);
            Assert.Equal("User", result?.Name);
        }

        [Fact]
        public async Task GetRoleByName_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetRoleByName("Unknown");
            Assert.Null(result);
        }
    }
}
