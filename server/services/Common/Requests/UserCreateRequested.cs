
namespace Common.Requests;

public class UserCreateRequested
{
    public UserCreateRequested(string name)
    {
        this.Name = name;
    }
    public string Name { get; set; } = null!;
}
