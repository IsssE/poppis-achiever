using GraphQL;
using GraphQL.Types;
using User.Types;
using MassTransit;
using GraphQL.Gateway.Types;
using Common.DTO;
using System.Net;
using static System.Net.WebRequestMethods;

namespace User;

public class UserQuery : ObjectGraphType<object>
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserQuery(UserHandler handler, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        Name = "Query";

        base.Field<UserType>("getUser")
        .Argument<NonNullGraphType<StringGraphType>>("id").Description("id of the user")
        .ResolveAsync(async ctx =>
        {
            var id = ctx.GetArgument<string>("id");
            var response = await handler.GetUserByIdAsync(id);
            return new UserDTO() { UserId = response.Message.UserId, DisplayName = response.Message.DisplayName };
        });

        Field<StringGraphType>("getToken")
        .Argument<NonNullGraphType<StringGraphType>>("userName")
        .Argument<NonNullGraphType<StringGraphType>>("password")
        .ResolveAsync(async ctx =>
        {
            var userName = ctx.GetArgument<string>("userName");
            var userPass = ctx.GetArgument<string>("password");
            var response = await handler.AuthenticateUser(userName, userPass);
            var httpCtx = _httpContextAccessor.HttpContext;

            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = false, // guess should do the real way, aka. with refresh token. maybe later :)
                SameSite = SameSiteMode.None,
                Path = "/",
                Secure = true, // Set to true if using HTTPS
            };
            httpCtx?.Response.Cookies.Append("jwt", $"Bearer {response.Message.Token}", options);


            return response.Message.Token;
        });

        Field<StringGraphType>("getIdForToken")
        .ResolveAsync(async ctx =>
        {
            var httpCtx = _httpContextAccessor.HttpContext;
            
            if (httpCtx == null || httpCtx.Request == null)
            {
                throw new Exception("ass");
            }
            string? authHeader = httpCtx.Request.Headers["Authorization"];
            if (authHeader == null || !authHeader.StartsWith("Bearer ")) return new Exception("authenticatio header faulty");
            string token = authHeader.Substring("Bearer ".Length).Trim();
            
            var res = await handler.RequestIdForToken(token);
            return res.Message.Id;

        });
    }
}