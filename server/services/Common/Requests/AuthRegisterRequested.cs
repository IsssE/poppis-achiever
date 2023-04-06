

namespace Common.Requests;

// need to see the correct way of implementing.
// Now it is cool, but bad to maintain.
// This request in the end uses another response
// the same as when requesting token
public class AuthRegisterRequested
{
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string? DisplayName { get; set; } = null;
}
