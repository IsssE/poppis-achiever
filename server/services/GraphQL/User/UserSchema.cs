using GraphQL.Instrumentation;
using GraphQL.Types;
using User;

namespace User;

public class UserSchema : Schema
{
    public UserSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetRequiredService<UserQuery>();
        Mutation = provider.GetRequiredService<UserMutation>();
        
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
