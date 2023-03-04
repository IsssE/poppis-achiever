using GraphQL;
using Kyykka;
using Kyykka.Types;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQL(builder => builder
    .AddSystemTextJson()
    .AddSchema<KyykkaSchema>()
    // Here for better error messages.
    .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true)
);

builder.Services.AddSingleton<KyykkaData>();
builder.Services.AddSingleton<KyykkaQuery>();
builder.Services.AddSingleton<UserType>();

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
await app.RunAsync();