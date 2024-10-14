using System.IdentityModel.Tokens.Jwt;
using Api.Permissions;
using Application.Features.Token;
using Application.Features.User;
using Common.Requests.User;
using Common.Wrappers;
using Infrastructure.Constant;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {

            IResponseWrapper response = await MediatorSender.Send(new CreateUserCommand{_createUserRequest=createUserRequest});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        [MustHavePermissionAttribute(AppRoleGroup.BasicAccess,AppFeature.Users,AppAction.Delete)]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            if(!await IsNormalUserRequest())
            {
                return BadRequest("IsNotNormalUserRequest");
            }

            if(!await IsNormalParameterRequest(AppJwtPayloadTypes.UserId,id))
            {
                return BadRequest("IsNotNormalParameterRequest");
            }
            
            DeleteUserRequest deleteUserRequest = new (){Id = id};
            IResponseWrapper response = await MediatorSender.Send(new DeleteUserCommand{_deleteUserRequest=deleteUserRequest});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        [MustHavePermissionAttribute(AppRoleGroup.BasicAccess, AppFeature.Users, AppAction.Update)]
        public async Task<IActionResult> UpdateUserUserAsync(string id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (!await IsNormalUserRequest())
            {
                return BadRequest("IsNotNormalUserRequest");
            }

            if (!await IsNormalParameterRequest(AppJwtPayloadTypes.UserId, id))
            {
                return BadRequest("IsNotNormalParameterRequest");
            }

            updateUserRequest.Id = id;
            IResponseWrapper response = await MediatorSender.Send(new UpdateUserCommand { _updateUserRequest = updateUserRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        [MustHavePermissionAttribute(AppRoleGroup.BasicAccess,AppFeature.Users,AppAction.Read)]        
        public async Task<IActionResult> SearchUserAsync(string id)
        {
            
            if(!await IsNormalUserRequest())
            {
                return BadRequest("IsNotNormalUserRequest");
            }

            if(!await IsNormalParameterRequest(AppJwtPayloadTypes.UserId,id))
            {
                return BadRequest("IsNotNormalParameterRequest");
            }
            
            SearchUserRequest searchUserRequest = new (){Id = id};
            IResponseWrapper response = await MediatorSender.Send(new SearchUserCommand{_searchUserRequest=searchUserRequest});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
