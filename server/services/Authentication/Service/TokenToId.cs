using Authentication.Data;
using Common.Requests;
using GraphQL;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using MassTransit.Context;
using System.Security.Claims;

namespace Authentication.Service;

public class TokenToId : IConsumer<TokenToIdRequested>
{
    private Data.AuthDbContext _dbContext;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public TokenToId(IConfiguration configuration, AuthDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _config = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    [Authorize]
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
