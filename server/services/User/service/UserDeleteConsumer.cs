using Common.DTO;
using Common.Requests;
using GraphQL;
using GraphQL.Types;
using MassTransit;
using System.Xml.Linq;

namespace User.Service;

public class UserDeleteConsumer : IConsumer<UserDeleteRequested>
{
    private Data.UserDbContext _dbContext;
    public UserDeleteConsumer(Data.UserDbContext context) {
        this._dbContext = context;
    }
    public Task Consume(ConsumeContext<UserDeleteRequested> context)
    {
        var dbVal = _dbContext.Users.FirstOrDefault(x => x.UserId == context.Message.Id);
        if (dbVal == null)
        {
            // TODO: see if this could be loaded as a meta information rather than thrown error that stops creation
            throw new ExecutionError($"Could not find a user with given id");
        }


        _dbContext.Remove(dbVal);
        _dbContext.SaveChanges();
        var deletedUser = new UserDTO() { UserId = context.Message.Id, DisplayName = string.Empty };
        context.RespondAsync(deletedUser);
        
        // context.RespondAsync(new UserDTO() {Id= dbVal.Id, Name = dbVal.Name});
        //context.Respond(new User { Id = "asd", Name = "asdasd" });

        return Task.CompletedTask;
    }
}