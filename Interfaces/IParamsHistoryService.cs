using System;
using banking.Entities;
using banking.Helpers;

namespace banking.Interfaces;

public interface IParamsHistoryService
{
    Task AddParamsHistory(ClientParams clientParams, int userId);
    Task<ClientParams> GetParamsHistory(int userId);
}
