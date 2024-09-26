using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using banking.Entities;
using banking.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace banking.Services;

public class TokenService(IConfiguration config) : ITokenService
{

    private readonly IConfiguration _config = config;

    // Create a JWT token for the user
    public string CreateToken(User user)
    {
        var signingCredentials = GetSigningCredentials(); // Get signing credentials for token
        var claims = GetClaims(user);                     // Get user-specific claims
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims); // Generate token options

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions); // Create and return JWT token
        return accessToken;
    }

    // Generate the JWT token options with claims and expiration
    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _config.GetSection("JwtSettings"); // Get JWT settings from config

        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["Expires"])), // Set token expiration
            signingCredentials: signingCredentials // Use provided signing credentials
        );

        return tokenOptions;
    }

    // Create the list of claims (ID, Role) for the user
    public List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID claim
            new(ClaimTypes.Role, user.Role),                   // User Role claim
        };
        return claims;
    }

    // Get signing credentials using the JWT secret key
    public SigningCredentials GetSigningCredentials()
    {
        var jwtConfig = _config.GetSection("JwtSettings"); // Get JWT settings from config
        var securityKey = jwtConfig["SecretKey"] ?? throw new Exception("The JWT secret key is not configured."); // Ensure key exists

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)); // Convert secret key to bytes

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256); // Return signing credentials
    }

    // Set the JWT token as a cookie in the response
    public void SetTokenCookie(string token, HttpContext context)
    {
        var jwtSettings = _config.GetSection("JwtSettings"); // Get JWT settings from config
        context.Response.Cookies.Append("accessToken", token, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["Expires"])), // Set cookie expiration
            HttpOnly = true,    // Cookie is only accessible via HTTP, not JavaScript
            Secure = true,      // Cookie is only sent over HTTPS
            SameSite = SameSiteMode.None // Cross-site cookie policy
        });
    }
}
