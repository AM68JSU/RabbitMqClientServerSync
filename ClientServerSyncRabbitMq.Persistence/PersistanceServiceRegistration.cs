

using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using ClientServerSyncRabbitMq.Persistence.Context;
using ClientServerSyncRabbitMq.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientServerSyncRabbitMq.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceService(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddDbContext<ServerDbContext>(options =>
       options.UseSqlServer(configuration.GetConnectionString("ServerDatabaseConnection")));

            services.AddDbContext<ClientDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ClientDatabaseConnection")));

            // در startup یا جایی که DI تنظیم می‌شود
            //services.AddScoped<ClientDbContext>();  // ثبت ClientDbContext
            //services.AddScoped<ServerDbContext>();  // ثبت ServerDbContext


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IUserRepository>(provider =>
            {
                var clientContext = provider.GetRequiredService<ClientDbContext>();
                return new UserRepository(clientContext); // می‌توانید از ClientDbContext یا ServerDbContext استفاده کنید
            });
            services.AddScoped<IUserRepository>(provider =>
            {
                var clientContext = provider.GetRequiredService<ServerDbContext>();
                return new UserRepository(clientContext); // می‌توانید از ClientDbContext یا ServerDbContext استفاده کنید
            });
            return services;
        }
    }
}
