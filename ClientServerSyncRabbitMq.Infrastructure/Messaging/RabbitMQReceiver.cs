using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Polly;
using System;
using System.Threading.Tasks;

public class RabbitMQReceiver : IAsyncDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQReceiver(IConnection connection)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
    }

    public void StartListening(string queueName, Func<string, Task> processMessage)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, context) =>
                {
                    Console.WriteLine($"Retrying message '{message}' due to: {exception.Message}. Waiting {timeSpan.TotalSeconds} seconds.");
                });

            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    await processMessage(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                });
            }
            catch (Exception ex)
            {
                _channel.BasicNack(ea.DeliveryTag, false, true);
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void StartSyncListener(Func<string, Task> processSyncMessage)
    {
        StartListening("ServerToClientQueue", processSyncMessage);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
            _channel.Close(); // تغییر به Close()

        if (_connection != null)
            _connection.Close(); // تغییر به Close()

        await Task.CompletedTask; // برای اینکه متد همچنان به صورت async باشد
    }
}
