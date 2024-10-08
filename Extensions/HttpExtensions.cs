using System;
using System.Text.Json;
using banking.Helpers;

namespace banking.Extensions;

public static class HttpExtensions
{
    // AddPaginationHeader is an extension method for the HttpResponse class 
    // that adds a pagination header to the response.
    public static void AddPaginationHeader<T>(this HttpResponse response, PagedList<T> data)
    {
        var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize, 
            data.TotalCount, data.TotalPages);

        var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}
