using HotChocolate.Authorization;

namespace SpareParts.API.Infrastructure
{
    public class AuthorizeByRoleHotChocAttribute : AuthorizeAttribute
    {
        public AuthorizeByRoleHotChocAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}
