using Purchases_Calculator.API.Domain;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Purchases_Calculator.API.Infrastructure.Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly IChannel _channel;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(IMessagingFactory messagingFactory, ILogger<MessagePublisher> logger)
    {
        // Use the factory to create the connection and channel
        var connection = messagingFactory.CreateConnectionAsync().Result;
        _channel = messagingFactory.CreateChannelAsync(connection).Result;
        _logger = logger;
    }

    public async Task PublishMessageAsync(string routingKey, Purchase purchase)
    {
        // Ensure the queue exists
        await _channel.QueueDeclareAsync(queue: "purchases", durable: true, exclusive: false, autoDelete: false);

        var message = JsonSerializer.Serialize(purchase);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        // Publish the message to the queue
        await _channel.BasicPublishAsync(exchange: "", routingKey: routingKey, mandatory: true, basicProperties: properties, body: body);

        _logger.LogInformation($"Sent message to {routingKey}: {message}");
    }
}