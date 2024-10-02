using RabbitMQ.Client;
using System;
using System.Text;

public class RabbitMQSender : IAsyncDisposable
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQSender(IConnection connection)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
    }

    public void SendMessage(string queueName, string message)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
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
