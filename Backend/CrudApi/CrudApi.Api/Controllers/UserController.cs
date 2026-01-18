using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var user = _userService.RegisterUser(request);
                return Ok(new { user.UserName, user.Email });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                string token = _userService.AuthenticateUser(request.UserName, request.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
