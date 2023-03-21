

namespace Common.Requests;

public class UserGetRequested
{
    public UserGetRequested(string id) {
        this.Id = id;   
    }
    public string Id { get; set; } = string.Empty;
}
