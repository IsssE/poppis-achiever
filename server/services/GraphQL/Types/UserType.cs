using Common.DTO;
using GraphQL.Types;

namespace User.Types;

public class UserType : ObjectGraphType<UserDTO>
{
    public UserType()
    {
        Name = "User";

        Field(h => h.UserId).Description("The id of the user.");
        Field(h => h.DisplayName, nullable: true).Description("The name of the user.");

        // Interface<UserInterface>();
    }
}