using System;
using System.Security.Cryptography;
using System.Text;
using banking.Data;
using banking.DTOs;
using banking.Entities;
using banking.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace banking.Repository;

// Handles user authentication (login and registration) related operations using Entity Framework Core
public class AuthenticationRepository(DataContext context) : IAuthenticationRepository
{
    private readonly DataContext _context = context; // Initialize the DataContext for database operations
    public async Task<User?> ValidateUser(LoginDto loginDto)
    {
        // Check if the username exists in the database (case-insensitive)
        var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == loginDto.Username.ToLower());
        if (userExist == null) return null; // Return null if the username does not exist

        using var hmac = new HMACSHA256(userExist.PasswordSalt); // Create an HMAC object using the stored salt
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // Compute the hash of the provided password

        for (int i = 0; i < computedHash.Length; i++) // Compare the computed hash with the stored hash
        {
            if (computedHash[i] != userExist.PasswordHash[i]) return null; // Return null if hashes do not match
        }

        return userExist; // Return the validated user object
    }

    public async Task<User?> RegisterAsync(RegisterDto registerDto)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == registerDto.Username.ToLower()); // Check if the username already exists (case-insensitive)
        if (userExists != null) return null; // Return null if the username already exists

         // Create an HMAC object to generate a new salt and hash for the password
        using var hmac = new HMACSHA256();
        var user = new User
        {
            Username = registerDto.Username,
            // Compute the hash of the provided password using the new salt
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), 
            // Store the generated salt for future use in validating the hashed password
            PasswordSalt = hmac.Key, 
            Role = registerDto.Role
        };
        await _context.Users.AddAsync(user); 
        await _context.SaveChangesAsync();
        return user; // Return the newly created (and saved) user object
    }
}
