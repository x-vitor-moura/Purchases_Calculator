namespace Purchases_Calculator.API.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message) where T : class;
}
