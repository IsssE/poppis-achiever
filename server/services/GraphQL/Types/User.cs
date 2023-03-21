namespace Kyykka.Types;

public class User
{
    public User(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

