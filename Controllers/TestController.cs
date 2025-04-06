using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("Only Admin can access this.");
        }

        [HttpGet("user-access")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult UserAndAdminAccess()
        {
            return Ok("Users and Admins can access this.");
        }
    }
}
