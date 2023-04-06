namespace Authentication.Service;

using Common.DTO;
using Common.Requests;
using MassTransit;
using MassTransit.Transports;
using BCrypt.Net;
using Common.Event;

public class AuthRegisterConsumer : IConsumer<AuthRegisterRequested>
{
    private Data.AuthDbContext _dbContext;
    private readonly IBusControl _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    public AuthRegisterConsumer(Data.AuthDbContext context, IBusControl busControl, ISendEndpointProvider sendEndpointProvider)
    {
        this._dbContext = context;
        _publishEndpoint = busControl;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Consume(ConsumeContext<AuthRegisterRequested> context)
    {
        var dbVal = _dbContext.Users.FirstOrDefault(x => x.UserId == context.Message.UserId);
        if (dbVal != null)
        {
            // TODO: see if this could be loaded as a meta information rather than thrown error that stops creation
            throw new GraphQL.ExecutionError($"There already is a user with the name: {context.Message.UserId}");
        }

        // TODO: HASH PASSWORD
        var hashPass = BCrypt.HashPassword(context.Message.Password);
        var newUser = new Models.UserAuthDetails() { UserId = context.Message.UserId, Password = hashPass };

        _dbContext.Add(newUser);
        _dbContext.SaveChanges();


        var display = context.Message.DisplayName == null ? context.Message.UserId : context.Message.DisplayName;
        _ = _publishEndpoint.Publish<UserCreateEvent>(new UserCreateEvent() { UserId = newUser.UserId, DisplayName = display });


        context.Respond(new AuthDTO() { UserId = newUser.UserId });

    }
}
