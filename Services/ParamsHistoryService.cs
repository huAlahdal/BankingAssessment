using System;
using System.Text.Json;
using banking.Data;
using banking.Entities;
using banking.Helpers;
using banking.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace banking.Services;

public class ParamsHistoryService(DataContext context) : IParamsHistoryService
{
    private readonly DataContext _context = context;

    public async Task AddParamsHistory(ClientParams clientParams, int userId)
    {
        // Create a new ClientParamsHistory object and populate with provided client parameters
        var newHistory = new ClientParamsHistory
        {
            UserId = userId,
            FirstName = clientParams.FirstName,
            LastName = clientParams.LastName,
            Sex = clientParams.Sex,
            OrderBy = clientParams.OrderBy,
            PageNumber = clientParams.PageNumber ?? 1, // Default to 1 if PageNumber is null
            PageSize = clientParams.PageSize ?? 10     // Default to 10 if PageSize is null
        };

        // Add the new history entry to the context
        await _context.ClientParamsHistory.AddAsync(newHistory);
        
        // Save changes to the database in a single transaction
        await _context.SaveChangesAsync();
    }

    public async Task<ClientParams> GetParamsHistory(int userId)
    {
        // Retrieve the most recent ClientParamsHistory entry for the given userId
        var historyParams = await _context.ClientParamsHistory
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Id)   // Order by Id to get the latest record
            .FirstOrDefaultAsync();         // Fetch the first result

        // If no history is found, return a new ClientParams with default values
        if (historyParams == null) 
            return new ClientParams{};

        // Return the retrieved history values as a new ClientParams object
        return new ClientParams
        {
            FirstName = historyParams.FirstName,
            LastName = historyParams.LastName,
            Sex = historyParams.Sex,
            OrderBy = historyParams.OrderBy,
            PageNumber = historyParams.PageNumber,
            PageSize = historyParams.PageSize
        };
    }
}
