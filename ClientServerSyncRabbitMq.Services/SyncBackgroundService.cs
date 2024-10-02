using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using Polly.Retry;
using AutoMapper;
using ClientServerSyncRabbitMq.Domain.Entities;
using ClientServerSyncRabbitMq.Application.DTOs;

public class SyncBackgroundService : BackgroundService
{
    private readonly RabbitMQReceiver _rabbitMQReceiver;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SyncBackgroundService> _logger;
    private readonly RabbitMQSender _rabbitMQSender;
    private readonly ConcurrentQueue<UserDto> _messageQueue = new ConcurrentQueue<UserDto>(); // تغییر به UserDto
    private bool _databaseConnected = true;

    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IMapper _mapper; // اضافه کردن AutoMapper

    public SyncBackgroundService(RabbitMQReceiver rabbitMQReceiver, IServiceScopeFactory scopeFactory, ILogger<SyncBackgroundService> logger, RabbitMQSender rabbitMQSender, IMapper mapper)
    {
        _rabbitMQReceiver = rabbitMQReceiver;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _rabbitMQSender = rabbitMQSender;
        _mapper = mapper;

        // تعریف Retry Policy برای استفاده مجدد
        _retryPolicy = Policy
            .Handle<DbUpdateConcurrencyException>()
            .Or<DbUpdateException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, context) =>
            {
                _logger.LogWarning($"Retrying due to error: {exception.Message}. Waiting {timeSpan.TotalSeconds} seconds before next retry.");
            });
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMQReceiver.StartListening("ClientToServerQueue", async (message) =>
        {
            await ProcessUserMessage(message);
        });

        _rabbitMQReceiver.StartSyncListener(async (message) =>
        {
            await ProcessSyncMessage(message);
        });

        return Task.CompletedTask;
    }

    private async Task ProcessUserMessage(string message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
            var userDto = JsonConvert.DeserializeObject<UserDto>(message); // تبدیل به UserDto

            var user = _mapper.Map<User>(userDto); // استفاده از AutoMapper برای تبدیل

            var userExist = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(c => c.SyncGuid == user.SyncGuid);

            try
            {
                if (userExist == null)
                {
                    await _retryPolicy.ExecuteAsync(async () =>
                    {
                        dbContext.Users.Add(user);
                        await dbContext.SaveChangesAsync();
                    });
                }
                else
                {
                    await _retryPolicy.ExecuteAsync(async () =>
                    {
                        userExist.Name = user.Name;
                        userExist.Email = user.Email;
                        dbContext.Users.Update(userExist);
                        await dbContext.SaveChangesAsync();
                    });
                }

                var syncMessage = JsonConvert.SerializeObject(new { user.SyncGuid, IsSynced = true });
                _rabbitMQSender.SendMessage("ServerToClientQueue", syncMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing user {SyncGuid}.", user.SyncGuid);
                _messageQueue.Enqueue(userDto); // استفاده از UserDto در صف
            }
        }
    }

    private async Task ProcessSyncMessage(string message)
    {
        var syncMessage = JsonConvert.DeserializeObject<dynamic>(message);
        Guid syncGuid = syncMessage.SyncGuid;
        bool isSynced = syncMessage.IsSynced;

        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ClientDbContext>();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.SyncGuid == syncGuid);

            if (user != null)
            {
                user.IsSynced = isSynced;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task ProcessMessageQueue()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
            var canConnect = await CanConnectToDatabaseAsync(dbContext);

            if (canConnect)
            {
                _databaseConnected = true;

                while (_messageQueue.TryDequeue(out var userDto))
                {
                    try
                    {
                        var user = _mapper.Map<User>(userDto); // تبدیل از UserDto به User
                        user.Id = 0;
                        await dbContext.Users.AddAsync(user);
                        await dbContext.SaveChangesAsync();

                        var syncMessage = JsonConvert.SerializeObject(new { user.SyncGuid, IsSynced = true });
                        _rabbitMQSender.SendMessage("ServerToClientQueue", syncMessage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process user {UserId} from queue.", userDto.SyncGuid);
                        _messageQueue.Enqueue(userDto);
                    }
                }
            }
            else if (_databaseConnected)
            {
                _logger.LogWarning("Lost connection to the database. Messages will be queued.");
                _databaseConnected = false;
            }
        }
    }

    private async Task<bool> CanConnectToDatabaseAsync(ServerDbContext dbContext)
    {
        try
        {
            await dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
