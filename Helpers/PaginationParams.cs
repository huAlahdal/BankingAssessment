using System;

namespace banking.Helpers;

public class PaginationParams
{
    private const int MaxPageSize = 50;
    public int? PageNumber { get; set; } = null;
    private int? _pageSize = null;

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
