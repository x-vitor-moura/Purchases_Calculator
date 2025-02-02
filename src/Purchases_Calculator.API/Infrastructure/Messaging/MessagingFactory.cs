using RabbitMQ.Client;

namespace Purchases_Calculator.API.Infrastructure.Messaging
{
    public class MessagingFactory : IMessagingFactory
    {
        private readonly RabbitMqSettings _rabbitMqSettings;

        public MessagingFactory(RabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
            Console.WriteLine($"RabbitMQ Host: {_rabbitMqSettings.HostName}");
        }

        public async Task<IConnection> CreateConnectionAsync()
        {

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.HostName,
                Port = _rabbitMqSettings.Port,
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password
            };

            return await factory.CreateConnectionAsync();
        }

        public async Task<IChannel> CreateChannelAsync(IConnection connection)
        {
            return await connection.CreateChannelAsync();
        }
    }
}
