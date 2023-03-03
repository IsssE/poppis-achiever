using GraphQL;
using GraphQL.Types;
using Kyykka;
using Kyykka.Types;

namespace Kyykka;

/// <example>
/// This is an example JSON request for a mutation
/// {
///   "query": "mutation ($user:userInput!){ createuser(user: $user) { id name } }",
///   "variables": {
///     "user": {
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

        Field<UserType, User>("createUser")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }))
        .Resolve(ctx =>
        {
            var user = ctx.GetArgument<User>("user");
            return data.AddUser(user);
        });
    }
}