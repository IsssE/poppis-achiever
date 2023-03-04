using GraphQL.Instrumentation;
using GraphQL.Types;

namespace Kyykka;

public class KyykkaSchema : Schema
{
    public KyykkaSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetRequiredService<KyykkaQuery>();
        // Mutation = (KyykkaMutation)provider.GetService(typeof(KyykkaMutation)) ?? throw new InvalidOperationException();

        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
