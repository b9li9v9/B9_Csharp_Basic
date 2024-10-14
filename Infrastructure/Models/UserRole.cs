
namespace Infrastructure.Models
{
    public class UserRole
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
