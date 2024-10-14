using Infrastructure.Constant;
using Microsoft.AspNetCore.Authorization;

namespace Api.Permissions
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string Group,string feature, string action)
            => Policy = AppPermission.NameFor(Group,feature, action);
    }
}
