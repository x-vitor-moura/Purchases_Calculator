using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace Purchases_Calculator.API.Infrastructure.Messaging;

public class RabbitMQMessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;

    public RabbitMQMessagePublisher(IRabbitMQConnection rabbitMQConnection)
    {
        _connection = rabbitMQConnection.CreateConnection();
    }

    public async Task PublishAsync<T>(T message) where T : class
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: "PURCHASE_CREATED", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);
            channel.BasicPublish(exchange: "", routingKey: "PURCHASE_CREATED", basicProperties: null, body: body);
        }
    }
}
