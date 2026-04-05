using MassTransit;
using Order.Application.Services;
using Order.Infrastructure;
using Order.Presentation;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.UseMessageRetry(r => r.Interval(10, TimeSpan.FromSeconds(5)));
    });
});

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices(builder.Configuration);
var app = builder.Build();

await InfrastructureServices.ApplyMigrationsAsync(app.Services);
OrderEndPoints.MapOrdersEndpoints(app);
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

