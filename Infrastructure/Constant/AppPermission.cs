using System.Collections.ObjectModel;

namespace Infrastructure.Constant
{
    public record AppPermission(string Feature, string Action, string Group, string Description, bool IsBasic = false)
    {
        public string Name => NameFor(Group,Feature, Action);

        public static string NameFor(string Group,string feature, string action)
        {
            return $"{AppClaim.Permission}.{Group}.{feature}.{action}";
        }
    }

    public class AppPermissions
    {
        //public static class Permissions
        //{
        //    public const string CreateUser = "CreateUsers";
        //    public const string UpdateUser = "UpdateUsers";
        //    public const string ReadUser = "ReadUsers";
        //    public const string DeleteUser = "DeleteUsers";
        //}

        private static readonly AppPermission[] _all = new AppPermission[]
        {
            new(AppFeature.Users, AppAction.Create, AppRoleGroup.SystemAccess, "Create Users"),
            new(AppFeature.Users, AppAction.Update, AppRoleGroup.SystemAccess, "Update Users"),
            new(AppFeature.Users, AppAction.Read, AppRoleGroup.SystemAccess, "Read Users"),
            new(AppFeature.Users, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Users"),

            new(AppFeature.UserRoles, AppAction.Read, AppRoleGroup.SystemAccess, "Read User Roles"),
            new(AppFeature.UserRoles, AppAction.Update, AppRoleGroup.SystemAccess, "Update User Roles"),

            new(AppFeature.Roles, AppAction.Read, AppRoleGroup.SystemAccess, "Read Roles"),
            new(AppFeature.Roles, AppAction.Create, AppRoleGroup.SystemAccess, "Create Roles"),
            new(AppFeature.Roles, AppAction.Update, AppRoleGroup.SystemAccess, "Update Roles"),
            new(AppFeature.Roles, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Roles"),

            new(AppFeature.RoleClaims, AppAction.Read, AppRoleGroup.SystemAccess, "Read Role Claims/Permissions"),
            new(AppFeature.RoleClaims, AppAction.Update, AppRoleGroup.SystemAccess, "Update Role Claims/Permissions"),

            new(AppFeature.Users, AppAction.Create, AppRoleGroup.BasicAccess, "Create Users",IsBasic: true),
            new(AppFeature.Users, AppAction.Update, AppRoleGroup.BasicAccess, "Update Users",IsBasic: true),
            new(AppFeature.Users, AppAction.Read, AppRoleGroup.BasicAccess, "Read Users",IsBasic: true),
            new(AppFeature.Users, AppAction.Delete, AppRoleGroup.BasicAccess, "Delete Users",IsBasic: true),

            new(AppFeature.Employees, AppAction.Read, AppRoleGroup.BasicAccess, "Read Employees", IsBasic: true),
            new(AppFeature.Employees, AppAction.Create, AppRoleGroup.BasicAccess, "Create Employees",IsBasic: true),
            new(AppFeature.Employees, AppAction.Update, AppRoleGroup.BasicAccess, "Update Employees",IsBasic: true),
            new(AppFeature.Employees, AppAction.Delete, AppRoleGroup.BasicAccess, "Delete Employees",IsBasic: true)
        };

        public static IReadOnlyList<AppPermission> AdminPermissions { get; } =
            new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsBasic).ToArray());

        public static IReadOnlyList<AppPermission> BasicPermissions { get; } =
            new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());

        public static IReadOnlyList<AppPermission> AllPermissions { get; } =
            new ReadOnlyCollection<AppPermission>(_all);
    }
}
