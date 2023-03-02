using GraphQL.Instrumentation;
using GraphQL.Types;

namespace Kyykka;

public class KyykkaSchema : Schema
{
    public KyykkaSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = (KyykkaQuery)provider.GetService(typeof(KyykkaQuery)) ?? throw new InvalidOperationException();
        Mutation = (KyykkaMutation)provider.GetService(typeof(KyykkaMutation)) ?? throw new InvalidOperationException();

        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}

public class DemoSchema : Schema
{
    public DemoSchema(IServiceProvider services) : base(services)
    {
        Query = services.GetRequiredService<KyykkaQuery>();
    }
}