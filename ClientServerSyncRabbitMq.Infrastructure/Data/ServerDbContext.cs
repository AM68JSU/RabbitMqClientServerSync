using Microsoft.EntityFrameworkCore;
using ClientServerSyncRabbitMq.Domain.Entities;
using System.Collections.Generic;

public class ServerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }
}
