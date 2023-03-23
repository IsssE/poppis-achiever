using Authentication;
using Authentication.Data;
using Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

/**
 Handles the stuff that is not visible for the user. 
Things like authentication and user roles. Might in the future enable
change of user groups
Somewhat overlaps with user service. This service should not care about users
name or achievemts. Just Id and password
 */
var builder = WebApplication.CreateBuilder(args);

bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

// Auth
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

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// Messaging
builder.Services.AddMassTransit(x =>
{
    var entryAssembly = Assembly.GetExecutingAssembly();
    x.AddConsumers(entryAssembly);
    var hostName = IsRunningInContainer ? RabbitMqConsts.RabbitMqHostName_container : RabbitMqConsts.RabbitMqHostName_local;
    x.UsingRabbitMq((context, busFactoryConfigurator) =>
    {
        busFactoryConfigurator.Host(hostName,
        "/",
        h =>
        {
            h.Username(RabbitMqConsts.UserName);
            h.Password(RabbitMqConsts.Password);
        });


        busFactoryConfigurator.ConfigureEndpoints(context);
    });
});

// DB context and connection
var connectionString = IsRunningInContainer ? "ConnectionStringContainer" : "ConnectionStringLocalDev";
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(connectionString)));

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AuthDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}


await app.RunAsync();