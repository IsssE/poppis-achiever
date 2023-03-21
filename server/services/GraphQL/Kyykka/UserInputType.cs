using GraphQL.Types;
using Kyykka.Types;

namespace Kyykka;

public class UserInputType : InputObjectGraphType<User>
{
    public UserInputType()
    {
        Name = "UserInput";
        Field(x => x.Name);
    }
}