using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class RoleClaim
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        
        public bool IsDeleted { get; set; } = false;

    }
}
