using System;
using System.ComponentModel.DataAnnotations;
using banking.Entities;

namespace banking.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "Gender must be either 'Admin' or 'User'.")]
    public string Role { get; set; } // 1 for user and 2 for admin
}
