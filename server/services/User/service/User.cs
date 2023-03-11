
using MassTransit;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
internal class UserHandler : ControllerBase
{
    protected readonly IBus _bus;

    public UserHandler(IBus bus)
    {
        _bus = bus;
    }
    public async Task<string> GetUser()
    {
            return await Task.Run(()=> "Tommi Linnamaa");
    }
}