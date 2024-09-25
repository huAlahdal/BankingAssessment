using System;

namespace banking.Entities;

public class SearchHistory
{
    public int Id { get; set;}
    public int UserId { get; set; }
    public string? PersonalId { get; set; }
    public DateTime SearchDate { get; set; } = DateTime.Now;
}


