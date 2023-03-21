using Common.DTO;
using Common.Requests;
using GraphQL;
using MassTransit;

namespace User.Service;

public class UserRequestedConsumer : IConsumer<UserGetRequested>
{
    private Data.UserDbContext _dbContext;
    public UserRequestedConsumer(Data.UserDbContext context)
    {
        this._dbContext = context;
    }

    public Task Consume(ConsumeContext<UserGetRequested> context)
    {
        var dbVal = _dbContext.Users.FirstOrDefault(x => x.Id == context.Message.Id);
        if(dbVal == null) {
            throw new ExecutionError($"Can't find user with id: {context.Message.Id}");
        }

        var result = new UserDTO() { Id = dbVal.Id, Name = dbVal.Name };
        context.RespondAsync(result);

        // context.RespondAsync(new UserDTO() {Id= dbVal.Id, Name = dbVal.Name});
        //context.Respond(new User { Id = "asd", Name = "asdasd" });

        return Task.CompletedTask;
    }
}

