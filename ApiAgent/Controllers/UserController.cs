using Common.Requests.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgent.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserUserAsync(string id, [FromBody] UpdateUserRequest updateUserRequest)
        {

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchUserAsync(string id)
        {

            return BadRequest();
        }

    }
}
