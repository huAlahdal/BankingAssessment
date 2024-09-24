using System;
using banking.DTOs;
using banking.Entities;

namespace banking.Interfaces;

public interface IAuthenticationRepository
{
    Task<User?> RegisterAsync(RegisterDto registerDto);
    Task<User?> ValidateUser(LoginDto loginDto);
}
