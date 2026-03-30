using Inventory.Application.Interfaces;
using Inventory.Application.services;
using Inventory.Domain.services;
using Inventory.Infrastructure.Messaging;
using Inventory.Infrastructure.Repositories;
using Inventory.Presentation;
using MassTransit;
using OrderSystem.Contracts.Common;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstant.InventoryOrderCreatedQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

builder.Services.AddScoped<IProductStockRepository, ProductStockRepository>();
builder.Services.AddScoped<InventoryDomainService, InventoryDomainService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var redisConfiguration = builder.Configuration["CacheSettings:ConnectionString"];
var redis = ConnectionMultiplexer.Connect(redisConfiguration);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
var app = builder.Build();
InventoryEndpoints.MapInventoryEndpoints(app);
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
