using GraphQL.Instrumentation;
using GraphQL.Types;
using Kyykka;

namespace Kyykka;

public class KyykkaSchema : Schema
{
    public KyykkaSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetRequiredService<KyykkaQuery>();
        Mutation = provider.GetRequiredService<KyykkaMutation>();
        
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
