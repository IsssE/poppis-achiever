using GraphQL;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Kyykka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pulu
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(b => b
                // .AddHttpMiddleware<ISchema>()
                // .AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddSchema<KyykkaSchema>()
                .AddGraphTypes(typeof(KyykkaSchema).Assembly));

            services.AddSingleton<KyykkaData>();
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // app.UseGraphQL<ISchema>();
            // app.UseGraphQLPlayground();
        }
    }
}