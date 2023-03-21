
namespace Common.Requests;

public class UserDeleteRequested
{
    public UserDeleteRequested(string id)
    {
        this.Id = id;
    }
    public string Id { get; set; } = string.Empty;
}

