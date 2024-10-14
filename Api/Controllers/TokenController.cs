using Application.Features.Token;
using Common.Requests.Token;
using Common.Wrappers;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TokenController : BaseController<TokenController>
    {
        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync([FromBody] CreateTokenRequest createTokenRequest)
        {
            //var ContentTypeValue = HttpContext.Request.Headers["Content-Type"];
            //return Ok(AuthorizationValue);

            IResponseWrapper response = await MediatorSender.Send(new CreateTokenCommand { _createTokenRequest = createTokenRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[MustHavePermissionAttribute(AppRoleGroup.BasicAccess,AppFeature.Users,AppAction.Update)]
        [HttpGet]
        public async Task<IActionResult> TestAsync()
        {
                return Ok("end");
        }
        
    }
}
