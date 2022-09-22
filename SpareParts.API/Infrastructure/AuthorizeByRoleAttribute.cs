using Microsoft.AspNetCore.Authorization;

namespace SpareParts.API.Infrastructure
{
    public class AuthorizeByRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeByRoleAttribute(params string[] roles) 
        {
            Roles = string.Join(",", roles);
        }
    }
}
