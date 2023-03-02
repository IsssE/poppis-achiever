
using Pulu;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureWebHost(builder => builder.UseStartup<Startup>())
            .Build()
            .RunAsync();
