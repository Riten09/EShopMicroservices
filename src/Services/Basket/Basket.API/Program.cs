using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);
var assembelly = typeof(Program).Assembly;
// Add services to the container

// Application releated service registeration
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembelly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Data releated service registeration
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName); //making username as Identity using Marten Schema attribute
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// manually adding decotor for IbasketRepo & IDistributedCache which we injected in CachedBasketRepo
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepo = provider.GetRequiredService<IBasketRepository>();
//    return new CachedBasketRepository(basketRepo, provider.GetRequiredService<IDistributedCache>());
//});

// doing registration of IBasketRepo & IdistributedCashe which injected in CachedBasketRepo using Scrutor library

builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//regitring for Redis 
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});
// gRPC releated service registeration
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        return handler;
    });

// Async Communication Service
builder.Services.AddMessageBroker(builder.Configuration); 

// handling exception/ Cross cutting services 
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
