using System;
using System.Text.Json;
using banking.DTOs;
using banking.Entities;
using Microsoft.EntityFrameworkCore;

namespace banking.Data;

public class Seed
{
    public static async Task SeedClients(DataContext context)
    {
        if (await context.Clients.AnyAsync()) return;

        var clientsData = await File.ReadAllTextAsync("Data/ClientsSeedData.json");

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        var clients = JsonSerializer.Deserialize<List<Client>>(clientsData, options);

        if (clients == null) return;

        
        foreach (var client in clients)
        {
            await context.Clients.AddAsync(client);
        }

        await context.SaveChangesAsync();
    }
}
