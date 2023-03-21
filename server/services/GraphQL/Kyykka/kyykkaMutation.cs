using Common.DTO;
using GraphQL;
using GraphQL.Types;
using GraphQLParser.AST;
using Kyykka.Types;

namespace Kyykka;

public class KyykkaMutation : ObjectGraphType
{
    public KyykkaMutation(KyykkaData data)
    {
        Name = "Mutation";

        Field<UserType>("createUser")
        .Argument<NonNullGraphType<StringGraphType>>("userName")
        .ResolveAsync( async ctx =>
        {
            var userName = ctx.GetArgument<string>("userName");
            var response = await data.AddUser(userName);
            return new User(response.Message.Id, response.Message.Name);
        });

        Field<UserType>("updateUser")
        .Argument<NonNullGraphType<StringGraphType>>("id")
        .Argument<NonNullGraphType<StringGraphType>>("newName")
        .ResolveAsync( async ctx => 
        {
            var id = ctx.GetArgument<string>("id");
            var newName = ctx.GetArgument<string>("newName");

            var response = await data.UpdateUserAsync(new User(id, newName));
            return new User(response.Message.Id, response.Message.Name);
        });

        Field<UserType>("deleteUser")
        .Argument<NonNullGraphType<StringGraphType>>("id")
        .ResolveAsync( async ctx => 
        {
            var id = ctx.GetArgument<string>("id");

            var response = await data.DeleteUserByIdAsync(id);
            return new User(response.Message.Id, string.Empty);
        });

    }
    
}