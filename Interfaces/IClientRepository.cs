using System;
using banking.DTOs;
using banking.Entities;

namespace banking.Interfaces;

public interface IClientRepository
{
    Task<List<ClientDto>> GetClients();
    Task<ClientDto?> GetClientById(int clientId);
    Task CreateClient(ClientDto client);
    void UpdateClient(int id, ClientDto client);
    Task<bool> RemoveClient(int clientId);
    Task<bool> SaveAllAsync();

}
