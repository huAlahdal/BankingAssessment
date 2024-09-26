using System;

namespace banking.DTOs;

public class UpdateAddressDto
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? ZipCode { get; set; }
}
