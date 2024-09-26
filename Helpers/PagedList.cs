using System;
using Microsoft.EntityFrameworkCore;

namespace banking.Helpers;

// Represents a paginated list of items, wrapping around a generic List<T>
public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int) Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    // Static method to create a PagedList from an IQueryable<T> using provided pagination parameters
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, 
        int pageSize)
    {
        var count = await source.CountAsync();
        // Skip and take the required items based on paginated offset and limit
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
