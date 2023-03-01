using Kyykka.Types;

namespace Kyykka;

public class KyykkaData
{

    private readonly List<User> _Users = new ();
    public KyykkaData()
    {
        _Users.Add(new User
        {
            Id = "1",
            Name = "Tommi Linnamaa",
        });
    }
    public User AddUser(User Users)
    {
        Users.Id = Guid.NewGuid().ToString();
        _Users.Add(Users);
        return Users;
    }
    public Task<User> GetUserByIdAsync(string id) => Task.FromResult(_Users.FirstOrDefault(h => h.Id == id));
}