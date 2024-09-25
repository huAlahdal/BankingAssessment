using System;
using banking.DTOs;
using banking.Entities;
using banking.Helpers;

namespace banking.Interfaces;

public interface IClientRepository
{
    Task<PagedList<ClientDto>> GetClients(ClientParams clientParams, int userId);
    Task<ClientDto?> GetClientByPersonalId(string personalId, int userId);
    Task<List<SearchHistoryDto>?> GetSearchSuggestions(int userId, int limit = 3);
    Task CreateClient(CreateClientDto client);
    Task<bool> UpdateClient(int id, UpdateClientDto updatedClient);
    Task<bool> RemoveClient(int clientId);
    Task<bool> SaveAllAsync();
    void Update(Client client);

}
