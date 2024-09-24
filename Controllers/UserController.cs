using banking.DTOs;
using banking.Entities;
using banking.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IAuthenticationRepository auth, ITokenService token) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            var user = await auth.RegisterAsync(registerDto);
            if (user == null) return BadRequest("User already exists");

            var accessToken = token.CreateToken(user);

            return Ok(accessToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            
            var user = await auth.ValidateUser(loginDto);
            if (user == null) return Unauthorized("Invalid username or password");
            var accessToken = token.CreateToken(user);
            if (accessToken == null) return BadRequest();
            return Ok(accessToken);
        }
    }
}
