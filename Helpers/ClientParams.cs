using System;
using banking.Entities;

namespace banking.Helpers;

public class ClientParams : PaginationParams
{
    public string? FirstName { get; set; }
    public string? LastName { get; set;}
    public string? Sex { get; set; }
    public string? OrderBy { get; set; }

    public bool AreAllPropertiesNull()
    {
        return FirstName == null && LastName == null && Sex == null && OrderBy == null && PageNumber == null && PageSize == null;
    }
}
