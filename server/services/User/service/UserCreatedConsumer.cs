using Common;
using MassTransit;
using Newtonsoft.Json;

namespace User.service
{
    public class UserCreatedConsumer : IConsumer<UserRequested>
    {
        public Task Consume(ConsumeContext<UserRequested> context)
        {
            context.RespondAsync(new UserDTO(Id: context.Message.Id, Name: "message response dude"));
            return Task.CompletedTask;
        }
    }
}
