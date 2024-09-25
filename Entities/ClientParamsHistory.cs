using System;

namespace banking.Entities;

public class ClientParamsHistory
{
    public int Id { get; set; }
    public int? UserId { get; set; } = null;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Sex { get; set; }
    public string? OrderBy { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public DateTime ParamsDate { get; set; } = DateTime.Now;
}

