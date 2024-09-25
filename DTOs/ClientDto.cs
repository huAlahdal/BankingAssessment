using System;
using System.ComponentModel.DataAnnotations;
using banking.Attributes;
using banking.Entities;
using PhoneNumbers;

namespace banking.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    public string PersonalId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNumber { get; set; }
    public string Sex { get; set; }
    public AddressDto Address { get; set; }
    public List<AccountDto> Accounts { get; set; }
}