using RabbitMQ.Client;

namespace Purchases_Calculator.API.Infrastructure.Messaging;

public interface IRabbitMQConnection
{
    IConnection CreateConnection();
}
