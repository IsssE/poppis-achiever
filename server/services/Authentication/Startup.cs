using Authentication.Data;
using Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;

namespace Authentication;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;

        // Auth
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer =   Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });

        services.AddAuthorization();
        services.AddEndpointsApiExplorer();

        // Messaging
        services.AddMassTransit(x =>
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
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString(connectionString)));
        
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        if (env.IsDevelopment())
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
    }
}
