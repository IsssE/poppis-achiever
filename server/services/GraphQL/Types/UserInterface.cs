using GraphQL.Types;

namespace Kyykka.Types;

public class UserInterface : InterfaceGraphType<User>
{
    public UserInterface()
    {
        Name = "User";

        Field(d => d.Id).Description("The id of the character.");
        Field(d => d.Name, nullable: true).Description("The name of the character.");
    }
}