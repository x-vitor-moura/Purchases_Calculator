using Purchases_Calculator.API.Domain;

namespace Purchases_Calculator.API.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublishMessageAsync(string routingKey, Purchase purchase);
}
