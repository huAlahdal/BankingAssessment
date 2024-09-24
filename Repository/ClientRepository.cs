using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using banking.Data;
using banking.DTOs;
using banking.Entities;
using banking.Interfaces;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;

namespace banking.Repository;

public class ClientRepository(DataContext context, IMapper mapper) : IClientRepository
{
    public async Task CreateClient(ClientDto client)
    {
        var clientMap = mapper.Map<Client>(client);
        await context.Clients.AddAsync(clientMap);
        await SaveAllAsync();
    }

    public async Task<ClientDto?> GetClientById(int clientId)
    {
        var client = await context.Clients.ProjectTo<ClientDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == clientId);
        if (client == null) return null;
        return client;
    }

    public async Task<List<ClientDto>> GetClients()
    {
        var clientDtos = await context.Clients.ProjectTo<ClientDto>(mapper.ConfigurationProvider).ToListAsync();
        return clientDtos;
    }

    public async Task<bool> RemoveClient(int clientId)
    {
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);
        if (client == null) return false;
        context.Clients.Remove(client);
        return await SaveAllAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateClient(int id, ClientDto client)
    {
        context.Entry(client).State = EntityState.Modified;
        // context.SaveChanges();
    }
}
