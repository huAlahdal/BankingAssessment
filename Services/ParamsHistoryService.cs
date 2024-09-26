using System;
using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        // Create a new ClientParamsHistory object with null-coalescing operator for defaults
        var newHistory = new ClientParamsHistory
        {
            UserId = userId,
            FirstName = clientParams.FirstName,
            LastName = clientParams.LastName,
            Sex = clientParams.Sex,
            OrderBy = clientParams.OrderBy,
            PageNumber = clientParams.PageNumber ?? 1,
            PageSize = clientParams.PageSize ?? 10
        };

        // Retrieve the last history entry and project necessary fields
        var lastHistory = await _context.ClientParamsHistory
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.ParamsDate)
            .Select(x => new { x.FirstName, x.LastName, x.Sex, x.OrderBy, x.PageNumber, x.PageSize })
            .FirstOrDefaultAsync();

        // Return if the new history matches the last one
        if (lastHistory != null && 
            clientParams.FirstName == lastHistory.FirstName &&
            clientParams.LastName == lastHistory.LastName &&
            clientParams.Sex == lastHistory.Sex &&
            clientParams.OrderBy == lastHistory.OrderBy &&
            clientParams.PageNumber == lastHistory.PageNumber &&
            clientParams.PageSize == lastHistory.PageSize)
        {
            return;
        }

        // Add the new history entry and save changes
        await _context.ClientParamsHistory.AddAsync(newHistory);
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
