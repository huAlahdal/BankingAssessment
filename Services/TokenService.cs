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
    public string CreateToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
		var claims = GetClaims(user);
		var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return accessToken;
    }

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = config.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["Expires"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    public List<Claim> GetClaims(User user)
    {
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role),
        };
        return claims;
    }

    public SigningCredentials GetSigningCredentials()
    {
        var jwtConfig = config.GetSection("JwtSettings");
        var securityKey = jwtConfig["SecretKey"] ?? throw new Exception("The JWT secret key is not configured.");

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    public void SetTokenCookie(string token, HttpContext context)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        context.Response.Cookies.Append("accessToken", token, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["Expires"])),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }
}
