using Common.DTO;
using Common.Requests;
using GraphQL;
using MassTransit;

namespace User.Service;

public class UserCreateConsumer : IConsumer<UserCreateRequested>
{
    private Data.UserDbContext _dbContext;
    public UserCreateConsumer(Data.UserDbContext context)
    {
        this._dbContext = context;
    }

    public Task Consume(ConsumeContext<UserCreateRequested> context)
    {
        var dbVal = _dbContext.Users.FirstOrDefault(x => x.Name == context.Message.Name);
        if (dbVal != null)
        {
            // TODO: see if this could be loaded as a meta information rather than thrown error that stops creation
            throw new ExecutionError($"There already is a user with the name: {context.Message.Name}");
        }

        var newId = Guid.NewGuid().ToString();
        var newUser = new Models.User() { Id = newId, Name = context.Message.Name };
        
        // Just a temp debugging value
        if (newUser.Name == "test")
        {
            newUser.Id = "-1";
        }

        _dbContext.Add(newUser);
        _dbContext.SaveChanges();

        var result = new UserDTO() { Id = newUser.Id, Name = newUser.Name };
        context.RespondAsync(result);
        
        // context.RespondAsync(new UserDTO() {Id= dbVal.Id, Name = dbVal.Name});
        //context.Respond(new User { Id = "asd", Name = "asdasd" });

        return Task.CompletedTask;
    }
}

