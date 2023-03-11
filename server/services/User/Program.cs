using MassTransit;
using MassTransit.DependencyInjection;
using System.Reflection;
using User.service;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    // Different tutorial ->
    // https://medium.com/multinetinventiv/publish-and-consume-messages-with-masstransit-and-rabbitmq-on-net-6-6118377bfedb
    var entryAssembly = Assembly.GetExecutingAssembly();
    x.AddConsumers(entryAssembly);
    x.UsingRabbitMq((context, busFactoryConfigurator) =>
    {
        busFactoryConfigurator.Host(new Uri(Common.RabbitMqConsts.RabbitMqUri), "/", h => {
            h.Username(Common.RabbitMqConsts.UserName);
            h.Password(Common.RabbitMqConsts.Password);
        });

        busFactoryConfigurator.ReceiveEndpoint("user-created-event", e =>
        {
            e.Consumer<UserCreatedConsumer>();
        });

        busFactoryConfigurator.ConfigureEndpoints(context);
    });

    //x.AddConsumer<UserCreatedConsumer>();
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
