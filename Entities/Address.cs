using System;

namespace banking.Entities;

public class Address
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
}
