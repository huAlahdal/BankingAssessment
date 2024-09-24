using System;
using System.ComponentModel.DataAnnotations;
using banking.Attributes;
using banking.Entities;
using PhoneNumbers;

namespace banking.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(11, ErrorMessage = "Personal ID must be 11 digits")]
    public string PersonalId { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

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
    [Range(1, 2)]
    public int Sex { get; set; }
    public AddressDto Address { get; set; }
    public List<AccountDto> Accounts { get; set; }
}