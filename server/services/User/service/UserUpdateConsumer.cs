
using Common.DTO;
using Common.Requests;
using MassTransit;
using User.Service;

namespace User.Service;
public class UserUpdateConsumer : IConsumer<UserUpdateRequested>
{
    private Data.UserDbContext _dbContext;
    public UserUpdateConsumer(Data.UserDbContext context)
    {
        this._dbContext = context;
    }
    public Task Consume(ConsumeContext<UserUpdateRequested> context)
    {
        var updatedUser = _dbContext.Users.FirstOrDefault(x => x.UserId == context.Message.Id);
        if (updatedUser == null)
        {
            // TODO: see if this could be loaded as a meta information rather than thrown error that stops creation
            throw new GraphQL.ExecutionError($"There already is a user with the name: {context.Message.Name}");
        }

        updatedUser.DisplayName = context.Message.Name;
        _dbContext.SaveChanges();

        var result = new UserDTO() { UserId = updatedUser.UserId, DisplayName = updatedUser.DisplayName };
        context.RespondAsync(result);

        return Task.CompletedTask;
    }
}