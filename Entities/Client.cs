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
    public string PersonalId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[]? ProfilePhoto { get; set; }
    public string MobileNumber { get; set; }    
    public Genders Sex { get; set; }
    public Address? Address { get; set; }
    public List<Account> Accounts { get; set; }
}
