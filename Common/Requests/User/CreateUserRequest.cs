//POST
namespace Common.Requests.User
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}