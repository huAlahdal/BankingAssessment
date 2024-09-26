using banking.DTOs;
using banking.Entities;
using banking.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace banking.Controllers
{
    // Define the route prefix for this controller
    [Route("api/[controller]")]
    // Indicate that this is an API controller
    [ApiController]
    public class UserController(IAuthenticationRepository auth, ITokenService token) : ControllerBase
    {
        private readonly IAuthenticationRepository _auth = auth; // Dependency injection of authentication repository
        private readonly ITokenService _token = token; // Dependency injection of token service

        // POST: api/user/register
        // Create a new user with the provided data and return an access token on success
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            var user = await _auth.RegisterAsync(registerDto);
            if (user == null) return BadRequest("User already exists"); // Return 400 Bad Request if user already exists

            var accessToken = _token.CreateToken(user);
            if (accessToken is not null) // Return a 200 OK response with the access token
            {
                return Ok(accessToken);
            }

            return BadRequest();
        }

        // POST: api/user/login
        // Validate user credentials and return an access token on success,
        // or return 401 Unauthorized if invalid, and 400 Bad Request if tokens cannot be generated
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var user = await _auth.ValidateUser(loginDto);
            if (user is null) return Unauthorized("Invalid username or password"); // Return 401 Unauthorized

            var accessToken = _token.CreateToken(user);
            if (accessToken is not null) // Return a 200 OK response with the access token
            {
                return Ok(accessToken);
            }

            return BadRequest(); // Return 400 Bad Request if tokens cannot be generated
        }
    }
}
