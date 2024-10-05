using System.Reflection;
using ClientServerSyncRabbitMq.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ClientServerSyncRabbitMq.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));  // با استفاده از Assembly.GetExecutingAssembly()


            return services;
        }
    }
}


