using System;

namespace banking.DTOs;

public class AddressDto
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int ZipCode { get; set; }
}
