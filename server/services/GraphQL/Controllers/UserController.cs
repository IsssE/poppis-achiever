using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLService.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public UserController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(Common.UserDTO userDTO)
        {
            // Temp as following tutorial. In future we want to get
            await _publishEndpoint.Publish<Common.UserRequested>(new
            {
                Id = 1,
            });
            return Ok();
        }
    }
}
