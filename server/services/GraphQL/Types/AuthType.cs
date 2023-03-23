using Common.DTO;
using GraphQL.Types;
using User.Types;

namespace GraphQL.Gateway.Types;

public class AuthType : ObjectGraphType<AuthDTO>
{
    public AuthType()
    {
        Name = "Auth";

        Field(h => h.UserId).Description("Unique Id for login");
        Field(h => h.Password, nullable: true).Description("Password for user");
        Field(h => h.DisplayName, nullable: true).Description("What ID will be shown for the user");
    }
}
