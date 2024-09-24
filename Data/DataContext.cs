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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Client>()
        // .HasMany(c => c.Accounts)
        // .WithOne(c => c.Client)
        // .HasForeignKey(c => c.ClientId);

        // modelBuilder.Entity<Account>()
        // .HasOne(c => c.Client)
        // .WithMany(c => c.Accounts)
        // .HasForeignKey(c => c.Id);
    }
}
