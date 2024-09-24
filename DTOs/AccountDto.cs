using System;

namespace banking.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    public string Number { get; set; }
    public decimal Balance { get; set; }
}
