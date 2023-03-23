using GraphQL;
using User;
using User.Types;
using MassTransit;
using GraphQL.Gateway.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQL(builder => builder
    .AddSystemTextJson()
    .AddSchema<UserSchema>()
    // Here for better error messages.
    .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true)
);
builder.Services.AddSingleton<UserHandler>();
builder.Services.AddSingleton<UserQuery>();
builder.Services.AddSingleton<UserType>();
builder.Services.AddSingleton<UserMutation>();
builder.Services.AddSingleton<UserInputType>();
builder.Services.AddSingleton<AuthType>();

builder.Services.AddHealthChecks();

bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

var hostName = IsRunningInContainer ? Common.RabbitMqConsts.RabbitMqHostName_container : Common.RabbitMqConsts.RabbitMqHostName_local;

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddMassTransit(mt => mt.AddMassTransit(x =>
{
    x.UsingRabbitMq((cntxt, cfg) =>
    {
        cfg.Host(hostName,
        "/",
        c =>
        {
            c.Username(Common.RabbitMqConsts.UserName);
            c.Password(Common.RabbitMqConsts.Password);
        });
    });
}));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_PuluHost",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                                              "https://localhost:5173", "http://getcloudshit");

                          //policy.AllowAnyHeader();
                          //policy.AllowAnyMethod();

                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowCredentials();

                      });

});


builder.Services.AddHttpContextAccessor();

var app = builder.Build();


//app.UseCors();
app.UseAuthentication();
//app.UseAuthorization();
app.UseCors("_PuluHost");
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