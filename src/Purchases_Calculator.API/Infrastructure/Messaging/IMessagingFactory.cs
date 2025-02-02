using RabbitMQ.Client;

namespace Purchases_Calculator.API.Infrastructure.Messaging
{
    public interface IMessagingFactory
    {
        Task<IConnection> CreateConnectionAsync();
        Task<IChannel> CreateChannelAsync(IConnection connection);
        Task DisposeConnectionAsync(IConnection connection);
    }
}
