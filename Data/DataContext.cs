using System;
using banking.Entities;
using Microsoft.EntityFrameworkCore;

namespace banking.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users{ get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Addresses { get; set; }
}
