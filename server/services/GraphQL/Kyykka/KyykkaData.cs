using Kyykka.Types;
using MassTransit.Clients;
using MassTransit;
using static GraphQL.Validation.Rules.OverlappingFieldsCanBeMerged;
using GraphQL;
using MassTransit.Transports;
using Common;

namespace Kyykka;

public class KyykkaData
{

    private readonly List<User> _Users = new();
    private readonly IBusControl _publishEndpoint;
    public KyykkaData(IBusControl busControl)
    {
        _publishEndpoint = busControl;
    }
    public User AddUser(User Users)
    {
        Users.Id = Guid.NewGuid().ToString();
        _Users.Add(Users);
        return Users;
    }
    public Task<Response<UserDTO>> GetUserByIdAsync(string id)
    {
        var result =  _publishEndpoint
            .CreateRequestClient<UserRequested>()
            .GetResponse<UserDTO>(new UserRequested (){ Id = id });
        
        return result;

    }
}