using GraphQL.Types;

namespace Kyykka.Types;

public class UserType : ObjectGraphType<User>
{
    public UserType()
    {
        Name = "User";

        Field(h => h.Id).Description("The id of the user.");
        Field(h => h.Name, nullable: true).Description("The name of the user.");

        // Interface<UserInterface>();
    }
}