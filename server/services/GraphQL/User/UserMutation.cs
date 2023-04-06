using Common.DTO;
using Common.Requests;
using GraphQL;
using GraphQL.Gateway.Types;
using GraphQL.Types;
using GraphQLParser.AST;
using User.Types;

namespace User;

public class UserMutation : ObjectGraphType
{
    public UserMutation(UserHandler handler)
    {
        Name = "Mutation";

        Field<AuthType>("registerUser")
        .Argument<NonNullGraphType<StringGraphType>>("userName")
        .Argument<NonNullGraphType<StringGraphType>>("password")
        .Argument<NonNullGraphType<StringGraphType>>("displayName")
        .ResolveAsync( async ctx =>
        {
            var userId = ctx.GetArgument<string>("userName");
            var userPass = ctx.GetArgument<string>("password");
            var displayName = ctx.GetArgument<string>("displayName");
            var response = await handler.RegisterUser(userId, userPass, displayName);

            return response.Message;
        });

        Field<UserType>("updateUser")
        .Argument<NonNullGraphType<StringGraphType>>("id")
        .Argument<NonNullGraphType<StringGraphType>>("newName")
        .ResolveAsync( async ctx => 
        {
            var id = ctx.GetArgument<string>("id");
            var newName = ctx.GetArgument<string>("newName");

            var response = await handler.UpdateUserAsync(id, newName);
            return response.Message;
        });

        //base.Field<UserType>("deleteUser")
        //.Argument<NonNullGraphType<StringGraphType>>("id")
        //.ResolveAsync( async ctx => 
        //{
        //    var id = ctx.GetArgument<string>("id");

        //    var response = await handler.DeleteUserByIdAsync(id);
        //    return new Types.User(response.Message.UserId, string.Empty);
        //});

    }
    
}