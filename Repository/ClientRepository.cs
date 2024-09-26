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
    private readonly DataContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IParamsHistoryService _paramsHistory = paramsHistory;

    public async Task CreateClient(CreateClientDto client)
    {
        var clientMap = _mapper.Map<Client>(client);
        await _context.Clients.AddAsync(clientMap);
        await SaveAllAsync();
    }

    public async Task<ClientDto?> GetClientByPersonalId(string personalId, int userId)
    {
        var client = await _context.Clients.ProjectTo<ClientDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.PersonalId == personalId);

        if (client != null)
        {
            // Check and add to search history in a separate query
            bool shouldAddToHistory = !await HasRecentSearch(personalId, userId);
            if (shouldAddToHistory)
            {
                await AddSearchHistoryAsync(userId, personalId);
            }
        }

        return client;
    }

    public async Task<PagedList<ClientDto>> GetClients(ClientParams clientParams, int userId)
    {
        // Initialize query to target the Clients table
        var query = _context.Clients.AsQueryable();

        // Apply filters only if client parameters are provided
        if (!string.IsNullOrWhiteSpace(clientParams.FirstName))
        {
            string lowerFirstName = clientParams.FirstName.ToLower();
            query = query.Where(x => x.FirstName.ToLower().Contains(lowerFirstName));
        }

        if (!string.IsNullOrWhiteSpace(clientParams.LastName))
        {
            string lowerLastName = clientParams.LastName.ToLower();
            query = query.Where(x => x.LastName.ToLower().Contains(lowerLastName));
        }

        if (!string.IsNullOrWhiteSpace(clientParams.Sex))
        {
            string lowerSex = clientParams.Sex.ToLower();
            query = query.Where(x => x.Sex.ToLower() == lowerSex);
        }

        // Apply ordering based on the provided 'OrderBy' parameter, default to ordering by 'Id'
        query = clientParams.OrderBy switch
        {
            "fisrtname" => query.OrderByDescending(x => x.FirstName),
            "lastname" => query.OrderByDescending(x => x.LastName),
            "sex" => query.OrderByDescending(x => x.Sex),
            "personalid" => query.OrderByDescending(x => x.PersonalId),
            _ => query.OrderBy(x => x.Id)
        };

        //save search parameters history for the user
        await _paramsHistory.AddParamsHistory(clientParams, userId);


        // Project the query to ClientDto and apply pagination before fetching the results
        return await PagedList<ClientDto>.CreateAsync(
            query.ProjectTo<ClientDto>(_mapper.ConfigurationProvider),  // Use AutoMapper's ProjectTo for projection
            clientParams.PageNumber ?? 1,  // Default to page 1 if no page number provided
            clientParams.PageSize ?? 10    // Default to page size of 10 if not provided
        );
    }


    public async Task<List<SearchHistoryDto>?> GetSearchSuggestions(int userId, int limit = 3)
    {
        return await _context.SearchHistories
        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.SearchDate)
        .ProjectTo<SearchHistoryDto>(_mapper.ConfigurationProvider)
        .Take(limit)
        .ToListAsync();
    }

    public async Task<bool> RemoveClient(int clientId)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);
        if (client == null) return false;
        _context.Clients.Remove(client);
        return await SaveAllAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateClient(int id, UpdateClientDto updatedClient)
    {
        var client = await _context.Clients.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);
        if (client == null) return false;

        // map new values to client
        _mapper.Map(updatedClient, client);

        if(await SaveAllAsync()) return true;
        
        return false;
    }

    public void Update(Client client)
    {
        _context.Entry(client).State = EntityState.Modified;
    }

    // Separate method to check if the current search is in recent searches
    private async Task<bool> HasRecentSearch(string personalId, int userId)
    {
        var recentSearches = await _context.SearchHistories
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.SearchDate)
            .Take(3)
            .ToListAsync();

        return recentSearches.Any(x => x.PersonalId == personalId);
    }

    // Separate method to add a new search history record
    private async Task AddSearchHistoryAsync(int userId, string personalId)
    {
        _context.SearchHistories.Add(new SearchHistory
        {
            UserId = userId,
            PersonalId = personalId,
            SearchDate = DateTime.UtcNow // Assuming you want to store the current date and time for each search
        });

        await _context.SaveChangesAsync();
    }
}
