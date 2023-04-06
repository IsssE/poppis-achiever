namespace Common.Requests;

public class UserUpdateRequested
{
    public UserUpdateRequested(string id, string name) { 
        this.Id = id;
        this.Name = name; 
    }
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}