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
    [Range(1, 2, ErrorMessage = "Invalid role. RoleId must be 1 for user or 2 for admin.")]
    public RoleEnum RoleId { get; set; } // 1 for user and 2 for admin
}
