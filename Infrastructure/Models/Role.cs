
namespace Infrastructure.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
}
