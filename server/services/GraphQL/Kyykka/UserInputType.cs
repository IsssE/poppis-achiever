using GraphQL.Types;
using Kyykka.Types;

namespace StarWars;

public class UserInputType : InputObjectGraphType<User>
{
    public UserInputType()
    {
        Name = "UserInput";
        Field(x => x.Name);
    }
}