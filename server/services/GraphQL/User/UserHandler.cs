using User.Types;
using MassTransit.Clients;
using MassTransit;
using static GraphQL.Validation.Rules.OverlappingFieldsCanBeMerged;
using GraphQL;
using MassTransit.Transports;
using Common.Requests;
using Common.DTO;
using GraphQL.Types;
using GraphQLParser.AST;
using GraphQL.Gateway.Types;

namespace User;

public class UserHandler
{
    private readonly IBusControl _publishEndpoint;
    public UserHandler(IBusControl busControl)
    {
        _publishEndpoint = busControl;
    }
    public Task<Response<AuthDTO>> RegisterUser(string userId, string password, string? displayName)
    {
        if (userId == null) { throw new InvalidDataException("creating user without name"); }

        var result = _publishEndpoint
            .CreateRequestClient<AuthRegisterRequested>()
            .GetResponse<AuthDTO>(new AuthRegisterRequested()
            {
                UserId = userId,
                Password = password,
                DisplayName = displayName
            });

        return result;
    }

    public Task<Response<AuthTokenResponse>> AuthenticateUser(string userName, string userPass)
    {
        if (userName == null) { throw new InvalidDataException("creating user without name"); }

        var result = _publishEndpoint
            .CreateRequestClient<AuthTokenRequested>()
            .GetResponse<AuthTokenResponse>(new AuthTokenRequested()
            {
                UserName = userName,
                Password = userPass,
            });

        return result;
    }

    public Task<Response<TokenToIdResponse>> RequestIdForToken(string token)
    {
        
        var result = _publishEndpoint
            .CreateRequestClient<TokenToIdRequested>()
            .GetResponse<TokenToIdResponse>(new TokenToIdRequested() { Token= token });

        return result;
    }

    public Task<Response<UserDTO>> GetUserByIdAsync(string id)
    {
        if (id == null) { throw new InvalidDataException("Requested user without giving a id"); }
        var result = _publishEndpoint
            .CreateRequestClient<UserGetRequested>()
            .GetResponse<UserDTO>(new UserGetRequested(id));

        return result;

    }
    public Task<Response<UserDTO>> UpdateUserAsync(string userId, string displayName)
    {
        var result = _publishEndpoint
                    .CreateRequestClient<UserUpdateRequested>()
                    .GetResponse<UserDTO>(new UserUpdateRequested(userId, displayName));

        return result;

    }
    public Task<Response<UserDTO>> DeleteUserByIdAsync(string id)
    {
        if (id == null) { throw new InvalidDataException("Requested user without giving a id"); }
        var result = _publishEndpoint
            .CreateRequestClient<UserDeleteRequested>()
            .GetResponse<UserDTO>(new UserDeleteRequested(id));

        return result;

    }
}