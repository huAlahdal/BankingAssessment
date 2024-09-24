using System;
using System.Security.Cryptography;
using System.Text;
using banking.Data;
using banking.DTOs;
using banking.Entities;
using banking.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace banking.Repository;

public class AuthenticationRepository(DataContext context) : IAuthenticationRepository
{
    public async Task<User?> ValidateUser(LoginDto loginDto)
    {
        var userExist = await context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == loginDto.Username.ToLower());
        if (userExist == null) return null;

        using var hmac = new HMACSHA256(userExist.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != userExist.PasswordHash[i]) return null;
        }

        return userExist;
        
    }

    public async Task<User?> RegisterAsync(RegisterDto registerDto)
    {
        var userExist = await context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == registerDto.Username.ToLower());
        if (userExist != null) return null;

        using var hmac = new HMACSHA256();

        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
            Role = registerDto.RoleId
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }
}
