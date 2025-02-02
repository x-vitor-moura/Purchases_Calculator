using Purchases_Calculator.API.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Purchases_Calculator.API.Infrastructure.Messaging;

public class MessageConsumer : BackgroundService, IMessageConsumer
{
    private readonly IChannel _channel;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IMessagingFactory messagingFactory, ILogger<MessageConsumer> logger)
    {
        var connection = messagingFactory.CreateConnectionAsync().Result;
        _channel = messagingFactory.CreateChannelAsync(connection).Result;
        _logger = logger;
    }

    public async Task StartConsumingAsync(CancellationToken stoppingToken)
    {
        await _channel.QueueDeclareAsync(queue: "purchases", durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {message}");

            var purchase = JsonSerializer.Deserialize<Purchase>(message);

            //just loggin the purchase, since not using for nothing
            _logger.LogInformation($"Purchases: {purchase}");

            // Acknowledge the message
            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await _channel.BasicConsumeAsync(queue: "purchases", autoAck: false, consumer: consumer);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartConsumingAsync(stoppingToken);
    }
}