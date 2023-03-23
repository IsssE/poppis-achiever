using Common.DTO;
using GraphQL.Types;
using User.Types;

namespace User;

public class UserInputType : InputObjectGraphType<UserDTO>
{
    public UserInputType()
    {
        Name = "UserInput";
        Field(x => x.DisplayName);
    }
}