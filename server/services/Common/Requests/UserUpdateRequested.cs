namespace Common.Requests;

public class UserUpdateRequested
{
    public UserUpdateRequested(string id, string name) { 
        this.Id = id;
        this.Name = name; 
    }
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}