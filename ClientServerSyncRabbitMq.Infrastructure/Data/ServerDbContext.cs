using Microsoft.EntityFrameworkCore;
using ClientServerSyncRabbitMq.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;

public class ServerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options) { }


    public class ServerDbContextFactory : IDesignTimeDbContextFactory<ServerDbContext>
    {
        public ServerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServerDbContext>();
            optionsBuilder.UseSqlServer("Password=123asd/;Persist Security Info=True;User ID=sa;Initial Catalog=ServerDB;Data Source=192.168.10.57;TrustServerCertificate=true;");

            return new ServerDbContext(optionsBuilder.Options);
        }
    }
}
