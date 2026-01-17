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

        // POST: api/users/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                var user = _userService.RegisterUser(request.UserName, request.Email, request.Password);
                return Ok(new { user.UserName, user.Email });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = _userService.AuthenticateUser(request.Email, request.Password);
                return Ok(new { user.UserName, user.Email });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
