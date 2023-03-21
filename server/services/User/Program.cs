using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using User.Data;
using User.Service;

var builder = WebApplication.CreateBuilder(args);

// Had swagger, removed for now
builder.Services.AddEndpointsApiExplorer();

bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

builder.Services.AddMassTransit(x =>
{
    var entryAssembly = Assembly.GetExecutingAssembly();
    x.AddConsumers(entryAssembly);
    var hostName = IsRunningInContainer ? Common.RabbitMqConsts.RabbitMqHostName_container : Common.RabbitMqConsts.RabbitMqHostName_local;
    x.UsingRabbitMq((context, busFactoryConfigurator) =>
    {
        busFactoryConfigurator.Host(hostName,
        "/",
        h =>
        {
            h.Username(Common.RabbitMqConsts.UserName);
            h.Password(Common.RabbitMqConsts.Password);
        });


        busFactoryConfigurator.ConfigureEndpoints(context);
    });

    x.AddConsumer<UserRequestedConsumer>();
    x.AddConsumer<UserCreateConsumer>();
    x.AddConsumer<UserDeleteConsumer>();
    x.AddConsumer<UserUpdateConsumer>();
});

var connectionString = IsRunningInContainer ? "ConnectionStringContainer" : "ConnectionStringLocalDev";

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(connectionString)));




var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UserDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
