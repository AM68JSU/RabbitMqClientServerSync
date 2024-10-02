using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// افزودن AutoMapper به DI container
builder.Services.AddAutoMapper(typeof(UserProfile)); // از پروفایل AutoMapper استفاده کنید

// Method for adding Database contexts with retry policy
void ConfigureDatabaseContexts(IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<ServerDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("ServerDatabase"),
            sqlOptions => sqlOptions.EnableRetryOnFailure()));

    services.AddDbContext<ClientDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("ClientDatabase"),
            sqlOptions => sqlOptions.EnableRetryOnFailure()));
}

// Method for configuring RabbitMQ
void ConfigureRabbitMQ(IServiceCollection services)
{
    // Singleton IConnection for RabbitMQ
    services.AddSingleton<IConnection>(provider =>
    {
        var factory = new ConnectionFactory() { HostName = "localhost" }; // آدرس RabbitMQ
        return factory.CreateConnection();
    });

    // Injecting RabbitMQReceiver and RabbitMQSender as singleton services
    services.AddSingleton<RabbitMQReceiver>(provider =>
    {
        var connection = provider.GetRequiredService<IConnection>();
        return new RabbitMQReceiver(connection);
    });

    services.AddSingleton<RabbitMQSender>(provider =>
    {
        var connection = provider.GetRequiredService<IConnection>();
        return new RabbitMQSender(connection);
    });
}

// Method for adding background services and other dependencies
void ConfigureServices(IServiceCollection services)
{
    // Registering the background service that handles sync and messaging
    services.AddHostedService<SyncBackgroundService>();

    // Registering the DBContext classes with scoped lifetime
    services.AddScoped<ServerDbContext>();
    services.AddScoped<ClientDbContext>();
}

// Calling configuration methods
ConfigureDatabaseContexts(builder.Services, builder.Configuration);
ConfigureRabbitMQ(builder.Services);
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
