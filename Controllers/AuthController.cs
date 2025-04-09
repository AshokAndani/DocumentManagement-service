using DocumentManagement.Entities;
using DocumentManagement.Models;
using DocumentManagement.Services;  
using DocumentManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentManagement.Controllers
{
    /// <summary>
    /// Provides Authentication and User Management routes.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="jwtTokenService"></param>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(
        IUserService userService,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
        : ControllerBase
    {

        /// <summary>
        /// OBSOLETE: Registers new user along with the role.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [Obsolete] // just to get an identity to authenticate
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> DummyRegister([FromBody] RegisterModel register)
        {
            if (register.UserName== null || register.Password== null || register.Role== null)
                return BadRequest();

            var roleObj = await userService.GetRoleByName(register.Role);
            if(roleObj == null)
                return BadRequest();
            await userService.AddUser(new User
            {
                Username = register.UserName,
                PasswordHash = passwordHasher.GetPasswordHash(register.Password),
                Email = register.UserName,
                RoleId = roleObj.Id,
                CreateTimestamp = DateTime.UtcNow,
                ModifyTimestamp = DateTime.UtcNow,
            });

            return Ok();
        }

        /// <summary>
        /// Authenticates the user based on passed in credentials.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (!(request != null && request.Username != null && request.Password != null))
                return Unauthorized();

            var user = await userService.GetByUserName(request.Username);
            if (user == null)
                return Unauthorized();

            var isPasswordValid = passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return Unauthorized();

            var token = jwtTokenService.GenerateToken(user.Username, user.Role!.Name!, user.Id);
            return Ok(new LoginResponse { Token = token});
        }
    }
}
