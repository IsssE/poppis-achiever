using Authentication.Data;
using Authentication.Models;
using Common.DTO;
using Common.Requests;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Service;

public class AuthTokenConsumer : IConsumer<AuthTokenRequested>
{
    private readonly IConfiguration _config;
    private AuthDbContext _dbContext;
    public AuthTokenConsumer(IConfiguration configuration, AuthDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _config = configuration;
        _dbContext = dbContext;
    }

    public Task Consume(ConsumeContext<AuthTokenRequested> context)
    {
        var response = new AuthTokenResponse();
        var user = new UserAuthDetails()
        {
            UserId = context.Message.UserName,
            Password = context.Message.Password
        };

        user = Authenticate(user);

        if (user != null)
        {
            var token = GenerateToken(user);

            response.Token = token;
        }
        context.Respond(response);

        return Task.CompletedTask;
    }

    //To authenticate user
    private UserAuthDetails? Authenticate(UserAuthDetails user)
    {
        // TODO: Password hashing
        var dbUser = _dbContext.Users.FirstOrDefault(x => x.UserId.ToLower() ==
        user.UserId.ToLower());

        if (dbUser != null && BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password))
        {
            return dbUser;
        }
        return null;
    }


    // To generate token
    private string GenerateToken(UserAuthDetails user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
        new Claim(type: ClaimTypes.NameIdentifier, user.UserId)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(7), // Prolly make this crazy big
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
