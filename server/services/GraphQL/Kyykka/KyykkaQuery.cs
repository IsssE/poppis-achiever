using GraphQL;
using GraphQL.Types;
using Kyykka.Types;
using MassTransit;

namespace Kyykka;

public class KyykkaQuery : ObjectGraphType<object>
{
    public KyykkaQuery(KyykkaData data)
    {
        Name = "Query";

        Field<UserType>("getUser_Messaging")
        .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the user" }
            ))
        .ResolveAsync(async ctx =>
        {
            var response = await data.GetUserByIdAsync("1");
            return new User()
            {
                Id = response.Message.Id,
                Name = response.Message.Name,
            };
        });
        
    }
}