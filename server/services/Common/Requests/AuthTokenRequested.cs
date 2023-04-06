namespace Common.Requests;

public class AuthTokenRequested
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
public class AuthTokenResponse
{
    public string Token { get; set; } = string.Empty;
}