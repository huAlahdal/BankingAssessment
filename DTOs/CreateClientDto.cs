using System;
using System.ComponentModel.DataAnnotations;
using banking.Attributes;
using banking.Entities;

namespace banking.DTOs;

public class CreateClientDto
{
    [Required]
    [StringLength(11, ErrorMessage = "Personal ID must be 11 digits")]
    [RegularExpression(@"\d{11}", ErrorMessage = "Personal ID must consist of exactly 11 digits.")]
    public string PersonalId { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [IsEmail]
    public string? Email { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(60)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(60)]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Mobile number is required")]
    [PhoneNumber]
    public string MobileNumber { get; set; }
    [Required]
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'male' or 'female'.")]
    public string Sex { get; set; }
    public AddressDto Address { get; set; }
    public List<AccountDto> Accounts { get; set; }
}
