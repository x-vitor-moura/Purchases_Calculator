using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Purchases_Calculator.API.Infrastructure.Databases;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;
using Purchases_Calculator.API.Infrastructure.Messaging;
using Serilog;
using System;

namespace Purchases_Calculator.API.Infrastructure.Configuration;

public static class ConfigureServices
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddSerilog();
        builder.AddSwagger();
        builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.AllowTrailingCommas = true;
        });
        builder.AddMessaging();
        builder.Services.AddTransient<IPurchaseRepository, PurchaseRepository>();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("StoreDb"));


    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
            // options.InferSecuritySchemes();
        });
    }

    private static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }


    private static void AddMessaging(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            var configuration = builder.Configuration;
            return new RabbitMQConnection(
                configuration["RabbitMQ:HostName"],
                int.Parse(configuration["RabbitMQ:Port"]),
                configuration["RabbitMQ:UserName"],
                configuration["RabbitMQ:Password"]);
        });

        builder.Services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();

    }


}
