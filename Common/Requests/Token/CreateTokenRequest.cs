using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Update search delete create
namespace Common.Requests.Token
{
    public class CreateTokenRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
