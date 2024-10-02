using Microsoft.EntityFrameworkCore;
using ClientServerSyncRabbitMq.Domain.Entities;
using System.Collections.Generic;

public class ClientDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options) { }
}
