namespace Purchases_Calculator.API.Infrastructure.Messaging;

public interface IMessageConsumer
{
    Task StartConsumingAsync(CancellationToken cancellationToken);
}