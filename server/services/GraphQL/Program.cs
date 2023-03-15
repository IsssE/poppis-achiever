using GraphQL;
using Kyykka;
using Kyykka.Types;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQL(builder => builder
    .AddSystemTextJson()
    .AddSchema<KyykkaSchema>()
    // Here for better error messages.
    .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true)
);

builder.Services.AddHealthChecks();

builder.Services.AddSingleton<KyykkaData>();
builder.Services.AddSingleton<KyykkaQuery>();
builder.Services.AddSingleton<UserType>();
builder.Services.AddSingleton<KyykkaMutation>();
builder.Services.AddSingleton<UserInputType>();

bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

var hostName = IsRunningInContainer ? Common.RabbitMqConsts.RabbitMqHostName_container : Common.RabbitMqConsts.RabbitMqHostName_local;
builder.Services.AddMassTransit(mt => mt.AddMassTransit(x => {
    x.UsingRabbitMq((cntxt, cfg) => {
        cfg.Host(hostName,
        "/", 
        c => {
            c.Username(Common.RabbitMqConsts.UserName);
            c.Password(Common.RabbitMqConsts.Password);
        });
    });
}));

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseWebSockets();
app.UseGraphQL("/graphql");            // url to host GraphQL endpoint
app.UseGraphQLPlayground(
    "/",                               // url to host Playground at
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",         // url of GraphQL endpoint
        SubscriptionsEndPoint = "/graphql",   // url of GraphQL endpoint
    });
//app.MapControllers();

await app.RunAsync();