using GraphQL;
using GraphQL.Types;
using Kyykka.Types;

namespace Kyykka;

public class KyykkaQuery : ObjectGraphType<object>
{
    public KyykkaQuery()
    {
        Name = "Query";

        Field<UserType>("user")
        .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the user" }
            ))
        .ResolveAsync(async ctx => await data.GetUserByIdAsync("1")
        );
    }
}