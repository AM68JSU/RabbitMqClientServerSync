using Microsoft.EntityFrameworkCore;
using ClientServerSyncRabbitMq.Domain.Entities;
using ClientServerSyncRabbitMq.Persistence.Configurations;
using Microsoft.EntityFrameworkCore.Design;

namespace ClientServerSyncRabbitMq.Persistence.Context
{
    public class ClientDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class ClientDbContextFactory : IDesignTimeDbContextFactory<ClientDbContext>
    {
        public ClientDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
            optionsBuilder.UseSqlServer("Password=123asd/;Persist Security Info=True;User ID=sa;Initial Catalog=ClientDB;Data Source=192.168.10.57;TrustServerCertificate=true;");

            return new ClientDbContext(optionsBuilder.Options);
        }
    }
}
