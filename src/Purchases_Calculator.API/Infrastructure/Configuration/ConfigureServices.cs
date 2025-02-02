using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Purchases_Calculator.API.Infrastructure.Databases;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;
using Purchases_Calculator.API.Infrastructure.Messaging;
using RabbitMQ.Client;
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
        builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value);

        builder.Services.AddSingleton<IMessagingFactory, MessagingFactory>();
        builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
        builder.Services.AddSingleton<IMessageConsumer, MessageConsumer>();
        builder.Services.AddHostedService<MessageConsumer>();

    }


}
