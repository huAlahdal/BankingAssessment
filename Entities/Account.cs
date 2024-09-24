using System;

namespace banking.Entities;

public class Account
{
    public int Id { get; set; }
    public string Number { get; set; }
    public decimal Balance { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
}
