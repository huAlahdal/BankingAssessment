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
    public async Task AddParamsHistory(ClientParams clientParams, int userId)
    {
        await context.ClientParamsHistory.AddAsync(new ClientParamsHistory
        {
            UserId = userId,
            FirstName = clientParams.FirstName,
            LastName = clientParams.LastName,
            Sex = clientParams.Sex,
            OrderBy = clientParams.OrderBy,
            PageNumber = clientParams.PageNumber ?? 1,
            PageSize = clientParams.PageSize ?? 10
        });
        await context.SaveChangesAsync();
    }

    public async Task<ClientParams> GetParamsHistory(int userId)
    {
        var historyParams = await context.ClientParamsHistory.OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.UserId == userId);
        if (historyParams == null) return new ClientParams{};
        return new ClientParams
        {
            FirstName = historyParams.FirstName,
            LastName = historyParams.LastName,
            Sex = historyParams.Sex,
            OrderBy = historyParams.OrderBy,
            PageNumber = historyParams.PageNumber ?? 1,
            PageSize = historyParams.PageSize ?? 10
        };
    }
}
