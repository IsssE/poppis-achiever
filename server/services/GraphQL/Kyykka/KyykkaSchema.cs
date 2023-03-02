using System;
using GraphQL.Instrumentation;
using GraphQL.Types;
using StarWars;

namespace Kyykka;

public class StarWarsSchema : Schema
{
    public StarWarsSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = (KyykkaQuery)provider.GetService(typeof(KyykkaQuery)) ?? throw new InvalidOperationException();
        Mutation = (KyykkaMutation)provider.GetService(typeof(KyykkaMutation)) ?? throw new InvalidOperationException();

        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}