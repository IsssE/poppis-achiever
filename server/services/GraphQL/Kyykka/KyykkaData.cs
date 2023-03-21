using Kyykka.Types;
using MassTransit.Clients;
using MassTransit;
using static GraphQL.Validation.Rules.OverlappingFieldsCanBeMerged;
using GraphQL;
using MassTransit.Transports;
using Common.Requests;
using Common.DTO;
using GraphQL.Types;
using GraphQLParser.AST;

namespace Kyykka;

public class KyykkaData
{

    // TODO: About these create + getresponse.
    // Could it be split to just senders and recievers.
    // How would the GraphQL then respond

    // See if we would do some caching. Now just fetch each time
    //private readonly List<User> _Users = new();

    private readonly IBusControl _publishEndpoint;
    public KyykkaData(IBusControl busControl)
    {
        _publishEndpoint = busControl;
    }
    public Task<Response<UserDTO>> AddUser(string userName)
    {
        if (userName == null) { throw new InvalidDataException("creating user without name"); }

        var result = _publishEndpoint
            .CreateRequestClient<UserCreateRequested>()
            .GetResponse<UserDTO>(new UserCreateRequested (userName));

        return result;
    }
    public Task<Response<UserDTO>> GetUserByIdAsync(string id)
    {
        if(id == null) { throw new InvalidDataException("Requested user without giving a id"); }
        var result =  _publishEndpoint
            .CreateRequestClient<UserGetRequested>()
            .GetResponse<UserDTO>(new UserGetRequested (id));
        
        return result;

    }
        public Task<Response<UserDTO>> UpdateUserAsync(User updatedUser)
    {
        var result = _publishEndpoint
                    .CreateRequestClient<UserUpdateRequested>()
                    .GetResponse<UserDTO>(new UserUpdateRequested(updatedUser.Id, updatedUser.Name));

        return result;

    }
      public Task<Response<UserDTO>> DeleteUserByIdAsync(string id)
    {
        if(id == null) { throw new InvalidDataException("Requested user without giving a id"); }
        var result =  _publishEndpoint
            .CreateRequestClient<UserDeleteRequested>()
            .GetResponse<UserDTO>(new UserDeleteRequested (id));
        
        return result;

    }
}