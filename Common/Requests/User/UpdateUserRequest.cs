using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 全部用PUT 部分用PATCH

namespace Common.Requests.User
{
    public class UpdateUserRequest
    {
        public string? Id { get; set; }
        public string Name { get; set; }
    }
}
