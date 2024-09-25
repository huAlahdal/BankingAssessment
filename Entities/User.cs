using System;
using System.ComponentModel.DataAnnotations;

namespace banking.Entities;

public enum RoleEnum
{
    [Display(Name = "User")]
    User = 1,
    [Display(Name = "Admin")]
    Admin = 2
}
public class User
{
    public int Id { get; set;}
    public required string Username { get; set;}
    public required byte[] PasswordHash { get; set;}
    public required byte[] PasswordSalt { get; set; }
    public required string Role { get; set; }

}
