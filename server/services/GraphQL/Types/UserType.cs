using GraphQL.Types;

namespace Kyykka.Types;

public class UserType : ObjectGraphType<User>
{
    public UserType(KyykkaData data)
    {
        Name = "User";

        Field(h => h.Id).Description("The id of the human.");
        Field(h => h.Name, nullable: true).Description("The name of the human.");

        Interface<UserInterface>();
    }
}