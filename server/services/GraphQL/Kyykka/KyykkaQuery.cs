using GraphQL;
using GraphQL.Types;
using Kyykka.Types;

namespace Kyykka;

public class KyykkaQuery : ObjectGraphType
{
    public KyykkaQuery()
    {
        Name = "Query";

        // Field<UserType>("user")
        // // .Arguments(new QueryArguments(
        // //         new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the user" }
        // //     ))
        // .ResolveAsync(async ctx => await data.GetUserByIdAsync(ctx.GetArgument<string>("1"))
        // );

        Field<StringGraphType>("hello")
        .Resolve(_ => "hello world");
    }
}