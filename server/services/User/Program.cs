using MassTransit;
using MassTransit.DependencyInjection;
using System.Reflection;
using User.service;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
        h => {
            h.Username(Common.RabbitMqConsts.UserName);
            h.Password(Common.RabbitMqConsts.Password);
        });


        busFactoryConfigurator.ConfigureEndpoints(context);
    });

    x.AddConsumer<UserCreatedConsumer>();
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.MapControllers();


app.Run();
