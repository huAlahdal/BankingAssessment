using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using banking.Data;
using banking.DTOs;
using banking.Entities;
using banking.Helpers;
using banking.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace banking.Repository;

public class ClientRepository(DataContext context, IMapper mapper, IParamsHistoryService paramsHistory) : IClientRepository
{
    public async Task CreateClient(CreateClientDto client)
    {
        var clientMap = mapper.Map<Client>(client);
        await context.Clients.AddAsync(clientMap);
        await SaveAllAsync();
    }

    public async Task<ClientDto?> GetClientByPersonalId(string personalId, int userId)
    {
        var client = await context.Clients.ProjectTo<ClientDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.PersonalId == personalId);

        // get 3 most recent searches for the user
        var recentSearches = await context.SearchHistories
        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.SearchDate)
        .ProjectTo<SearchHistoryDto>(mapper.ConfigurationProvider)
        .Take(3)
        .ToListAsync();

        // if the current search is not in the search history, add it to the search history
        if (client != null && !recentSearches.Any(x => x.PersonalId == personalId)) 
        {
            // Add the current search to the search history
            context.SearchHistories.Add(new SearchHistory
            {
                UserId = userId, // Replace with actual user ID
                PersonalId = personalId
            });

            await context.SaveChangesAsync();
        }
        

        return client;
    }

    public async Task<PagedList<ClientDto>> GetClients(ClientParams clientParams, int userId)
    {
        var query = context.Clients.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(clientParams.FirstName))
            query = query.Where(x => x.FirstName.ToLower().Contains(clientParams.FirstName.ToLower()));
        
        if (!string.IsNullOrWhiteSpace(clientParams.LastName))
            query = query.Where(x => x.LastName.ToLower().Contains(clientParams.LastName.ToLower()));

        if (!string.IsNullOrWhiteSpace(clientParams.Sex))
            query = query.Where(x => x.Sex.ToLower() == clientParams.Sex.ToLower());

        // Apply ordering
        query = clientParams.OrderBy switch
        {
            "FirstName" => query.OrderByDescending(x => x.FirstName),
            "LastName" => query.OrderByDescending(x => x.LastName),
            "Sex" => query.OrderByDescending(x => x.Sex),
            "PersonalId" => query.OrderByDescending(x => x.PersonalId),
            _ => query.OrderBy(x => x.Id)
        };

        // Save search history if clientParams provided
        if (!clientParams.AreAllPropertiesNull())
            await paramsHistory.AddParamsHistory(clientParams, userId);

        // Return paginated result
        return await PagedList<ClientDto>.CreateAsync(
            query.ProjectTo<ClientDto>(mapper.ConfigurationProvider),
            clientParams.PageNumber ?? 1,
            clientParams.PageSize ?? 10
        );
    }

    public async Task<List<SearchHistoryDto>?> GetSearchSuggestions(int userId, int limit = 3)
    {
        return await context.SearchHistories
        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.SearchDate)
        .ProjectTo<SearchHistoryDto>(mapper.ConfigurationProvider)
        .Take(limit)
        .ToListAsync();
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

    public async Task<bool> UpdateClient(int id, UpdateClientDto updatedClient)
    {
        Console.WriteLine(JsonSerializer.Serialize(updatedClient));
        var client = await context.Clients.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);
        if (client == null) return false;

        // map new values to client
        mapper.Map(updatedClient, client);

        Update(client);

        if(await SaveAllAsync()) return true;
        
        return false;
    }

    public void Update(Client client)
    {
        context.Entry(client).State = EntityState.Modified;
    }
}
