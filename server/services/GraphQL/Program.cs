using GraphQL;
using Kyykka;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQL(builder => builder
    .AddSystemTextJson()
    .AddSchema<DemoSchema>()
);

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseWebSockets();
app.UseGraphQL<DemoSchema>();
app.UseGraphQL("/graphql");            // url to host GraphQL endpoint
app.UseGraphQLPlayground(
    "/",                               // url to host Playground at
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",         // url of GraphQL endpoint
        SubscriptionsEndPoint = "/graphql",   // url of GraphQL endpoint
    });
await app.RunAsync();