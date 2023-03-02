using System;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Kyykka;

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