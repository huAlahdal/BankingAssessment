using System;
using System.ComponentModel.DataAnnotations;
using banking.Attributes;
using banking.Entities;

namespace banking.DTOs;

public class UpdateClientDto
{
    [StringLength(11, ErrorMessage = "Personal ID must be 11 digits")]
    [RegularExpression(@"\d{11}", ErrorMessage = "Personal ID must consist of exactly 11 digits.")]
    public string? PersonalId { get; set; }
    [IsEmail]
    public string? Email { get; set; }
    [MaxLength(60)]
    public string? FirstName { get; set; }
    [MaxLength(60)]
    public string? LastName { get; set; }
    [PhoneNumber]
    public string? MobileNumber { get; set; }
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
    public string? Sex { get; set; }
    public AddressDto? Address { get; set; }
}
