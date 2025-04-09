using DocumentManagement.Controllers;
using DocumentManagement.Entities;
using DocumentManagement.Models;
using DocumentManagement.Services;
using DocumentManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DocumentManagementTests.ControllerTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();

            var inMemorySettings = new Dictionary<string, string> 
            {
                {
                    "JwtOptions:SecretKey", "supersecretkey12345"
                }
            };

            _controller = new AuthController(_userServiceMock.Object, _passwordHasherMock.Object, _jwtTokenServiceMock.Object);
        }
        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenRequestIsNull()
        {
            var result = await _controller.Login(null!);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserIsNotFound()
        {
            _userServiceMock.Setup(s => s.GetByUserName("user1")).ReturnsAsync((User?)null);

            var result = await _controller.Login(new LoginModel
            {
                Username = "user1",
                Password = "pass"
            });

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsInvalid()
        {
            var user = new User
            {
                Username = "user1",
                PasswordHash = "hashed",
                Role = new Role { Name = "Admin" },
                Id = 1
            };

            _userServiceMock.Setup(s => s.GetByUserName("user1")).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.VerifyPasswordHash("pass", "hashed")).Returns(false);

            var result = await _controller.Login(new LoginModel
            {
                Username = "user1",
                Password = "pass"
            });

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken_WhenValidCredentials()
        {
            var user = new User
            {
                Username = "user",
                PasswordHash = "hashed",
                Role = new Role { Name = "Admin" },
                Id = 1
            };

            _userServiceMock.Setup(x => x.GetByUserName("user")).ReturnsAsync(user);
            _passwordHasherMock.Setup(x => x.VerifyPasswordHash("pass", "hashed")).Returns(true);
            _jwtTokenServiceMock.Setup(x => x.GenerateToken("user", "Admin", 1)).Returns("token123");

            var result = await _controller.Login(new LoginModel { Username = "user", Password = "pass" });

            var okResult = Assert.IsType<OkObjectResult>(result);
            LoginResponse? value = okResult.Value as LoginResponse;
            Assert.Equal("token123", value?.Token);
        }
    }
}
