using Common.Requests;
using MassTransit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.Service;

public class TokenToId : IConsumer<TokenToIdRequested>
{
    public TokenToId()
    {
    }

    public Task Consume(ConsumeContext<TokenToIdRequested> context)
    {

        var authToken = context.Message.Token;
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.ReadJwtToken(authToken);
        var username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (username != null)
        {
            context.Respond(new TokenToIdResponse() { Id = username.Value });
        }

        return Task.CompletedTask;
    }
}
