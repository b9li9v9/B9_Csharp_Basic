using Infrastructure.Models;

namespace Infrastructure.Constant
{
    public static class AppJwtPayloadTypes
    {
        public const string UserId = nameof(UserId);
        public const string UserEmail = nameof(UserEmail);
        public const string Roles = nameof(Roles);
        public const string Permission = nameof(Permission);
    }
}
