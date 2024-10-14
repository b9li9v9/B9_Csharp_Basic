using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Context;
using Infrastructure.Constant;

namespace Api.Controllers;
public class BaseController<T>: ControllerBase
{
    
    private ISender _sender;
    public ISender MediatorSender => _sender ??= HttpContext.RequestServices.GetService<ISender>();

    private ILogger<T> _logger;
    public ILogger<T> logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();

    private ApplicationDbContext _applicationDbContext;
    
    public ApplicationDbContext applicationDbContext => _applicationDbContext ??= HttpContext.RequestServices.GetService<ApplicationDbContext>();

    // 请求用户是否正常 
    protected async Task<bool> IsNormalUserRequest()
    {
        
        var claims = await LoadJwtTokenClaims();
        if(claims is not null)
        {
            var userId = claims.FirstOrDefault(c => c.Type == AppJwtPayloadTypes.UserId)?.Value;
            if(applicationDbContext.Users.Any(e => e.Id == userId))
            {
                return true;
            }
            return false;
        }

        return false;
    } 
    
    // 请求参数确权（去匹配自身的Token）
    protected async Task<bool> IsNormalParameterRequest(string AppJwtPayloadTypesMember,string Parameter)
    {
        
        var claims = await LoadJwtTokenClaims();
        if(claims is not null)
        {
            var appJwtPayloadValue = claims.FirstOrDefault(c => c.Type == AppJwtPayloadTypesMember)?.Value;
            return appJwtPayloadValue == Parameter;
        }
        return false;
    }

    // 从请求头[Ahtu..]解析jwt
    protected async Task<IEnumerable<System.Security.Claims.Claim>> LoadJwtTokenClaims()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // 解析JWT
                var handler = new JwtSecurityTokenHandler();

                // 如果令牌有效，处理 token 信息
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);

                    // 获取JWT中的声明(claims)
                    var claims = jwtToken.Claims.ToList();
                    return claims;
                    // // 打印所有声明
                    // foreach (var claim in claims)
                    // {
                    //     logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                    // }

                    // 拿到jwt里指定的type的值
                    // var appJwtPayloadValue = claims.FirstOrDefault(c => c.Type == appJwtPayloadTypes)?.Value;
                    // 和URL请求值进行配对 确权这个操作的是它自己的账号
                    // return appJwtPayloadValue == requestVuale;
                }
                else
                {
                    logger.LogWarning("Invalid JWT Token.");
                    return null;
                }
            }
            else
            {
                logger.LogWarning("Authorization header is missing or invalid.");
                return null;
            }

    }
}