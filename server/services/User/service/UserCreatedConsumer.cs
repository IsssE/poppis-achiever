using Common;
using GraphQL;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace User.service;

public class UserRequestedConsumer : IConsumer<UserRequested>
{
    private UserDbContext _dbContext;
    public UserRequestedConsumer(UserDbContext context)
    {
        this._dbContext = context;
    }

    public Task Consume(ConsumeContext<UserRequested> context)
    {
        var dbVal = _dbContext.Users.FirstOrDefault(x => x.Id == context.Message.Id);
        if(dbVal != null) {
            var result = new UserDTO() {Id= dbVal.Id, Name = dbVal.Name};
            context.RespondAsync(result);
        }
        else {
            throw new ExecutionError($"Can't find user with id: {context.Message.Id}");
        }
        // context.RespondAsync(new UserDTO() {Id= dbVal.Id, Name = dbVal.Name});
        //context.Respond(new User { Id = "asd", Name = "asdasd" });
        
        return Task.CompletedTask;
    }
}

