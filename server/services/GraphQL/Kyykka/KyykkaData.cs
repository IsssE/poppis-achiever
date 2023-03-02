using Kyykka.Types;

namespace Kyykka;

public class KyykkaData
{

    private readonly List<User?> _Users = new List<User?>();
    public KyykkaData()
    {
        

        _Users.Add(new User
        {
            Id = "1",
            Name = "Tommi Linnamaa",
        });
    }
    public User AddHuman(User Users)
    {
        Users.Id = Guid.NewGuid().ToString();
        _Users.Add(Users);
        return Users;
    }
    public Task<User?> GetHumanByIdAsync(string id) => Task.FromResult(_Users.FirstOrDefault(h => h.Id == id));
}