using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Purchases_Calculator.API.Infrastructure.Messaging;

public class RabbitMQConnection : IRabbitMQConnection
{
    private readonly string _hostName;
    private readonly int _port;
    private readonly string _userName;
    private readonly string _password;
    private readonly IConnection _connection;

    public RabbitMQConnection(string hostName, int port, string userName, string password)
    {
        _hostName = hostName;
        _port = port;
        _userName = userName;
        _password = password;
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();
        }
        catch (BrokerUnreachableException ex)
        {
            Console.WriteLine($"Broker unreachable: {ex.Message}");
            throw; // Re-throw the exception to signal a critical issue
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
            throw; // Re-throw the exception
        }
    }

    public IConnection CreateConnection()
    {
        return _connection;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
