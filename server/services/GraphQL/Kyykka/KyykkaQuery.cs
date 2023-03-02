using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Kyykka.Types;

namespace Kyykka;

public class KyykkaQuery : ObjectGraphType<object>
{
    public KyykkaQuery(KyykkaData data)
    {
        Name = "Query";

        Field<UserType>("human")
        .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
            ))
        .ResolveAsync(async ctx => await data.GetHumanByIdAsync(ctx.GetArgument<string>("id"))
        );
    }
}