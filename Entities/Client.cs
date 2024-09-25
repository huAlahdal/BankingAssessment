using System;
using System.ComponentModel.DataAnnotations;

namespace banking.Entities;

public enum Genders
{
    [Display(Name = "Male")]
    MALE = 1,
    [Display(Name = "Female")]
    FEMALE = 2
}
public class Client
{
    public int Id { get; set; }
    public required string PersonalId { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public byte[]? ProfilePhoto { get; set; }
    public required string MobileNumber { get; set; }    
    public required string Sex { get; set; }
    public required Address Address { get; set; }
    public required List<Account> Accounts { get; set; }
}
