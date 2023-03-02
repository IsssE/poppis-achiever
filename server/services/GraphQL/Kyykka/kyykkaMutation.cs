using GraphQL;
using GraphQL.Types;
using Kyykka;
using Kyykka.Types;

namespace Kyykka;

/// <example>
/// This is an example JSON request for a mutation
/// {
///   "query": "mutation ($human:HumanInput!){ createHuman(human: $human) { id name } }",
///   "variables": {
///     "human": {
///       "name": "Boba Fett"
///     }
///   }
/// }
/// </example>
public class KyykkaMutation : ObjectGraphType
{
    public KyykkaMutation(KyykkaData data)
    {
        Name = "Mutation";


        Field<UserType, User>("createHuman")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<UserInputType>> { Name = "human" }))
        .Resolve(ctx =>
        {
            var user = ctx.GetArgument<User>("user");
            return data.AddHuman(user);
        });
    }
}