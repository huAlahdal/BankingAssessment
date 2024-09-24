using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using banking.Entities;
using Microsoft.IdentityModel.Tokens;

namespace banking.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
    SigningCredentials GetSigningCredentials();
    JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    List<Claim> GetClaims(User user);
    void SetTokenCookie(string token, HttpContext context);
}
