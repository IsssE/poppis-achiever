using Common.DTO;
using Common.Event;
using Common.Requests;
using GraphQL;
using MassTransit;

namespace User.Service;

public class UserCreateConsumer : IConsumer<UserCreateEvent>
{
    private Data.UserDbContext _dbContext;
    public UserCreateConsumer(Data.UserDbContext context)
    {
        this._dbContext = context;
    }

    public Task Consume(ConsumeContext<UserCreateEvent> context)
    {
        var dbVal = _dbContext.Users.FirstOrDefault(x => x.DisplayName.ToLower() == context.Message.UserId.ToLower());
        if (dbVal != null)
        {
            // TODO: see if this could be loaded as a meta information rather than thrown error that stops creation
            throw new ExecutionError($"There already is a user with the name: {context.Message.UserId}");
        }
        var displayName = context.Message.DisplayName ?? context.Message.UserId;
        var newUser = new Models.User() { UserId = context.Message.UserId, DisplayName = displayName };
        
        // Just a temp debugging value
        if (newUser.DisplayName == "test")
        {
            newUser.UserId = "-1";
        }

        _dbContext.Add(newUser);
        _dbContext.SaveChanges();

        var result = new UserDTO() { UserId = newUser.UserId, DisplayName = displayName };
        context.RespondAsync(result);
        
        // context.RespondAsync(new UserDTO() {Id= dbVal.Id, Name = dbVal.Name});
        //context.Respond(new User { Id = "asd", Name = "asdasd" });

        return Task.CompletedTask;
    }
}

