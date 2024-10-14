using Infrastructure.Constant;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Api.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger<PermissionAuthorizationHandler> _logger;
        public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger) 
        { 
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            _logger.LogWarning("HandleRequirementAsync");

            if (context.User is null)
            {
                _logger.LogError("context.User is null ");

                await Task.CompletedTask;
                return;
            }

            // if (!context.User.Identity.IsAuthenticated)
            // {
            //     _logger.LogError("!context.User.Identity.IsAuthenticated");

            //     return Task.CompletedTask;
            // }

            var permissions = context.User.Claims
                .Where(claim => claim.Type == AppClaim.Permission
                    && claim.Value == requirement.Permission);

            // foreach(var c in context.User.Claims)
            // {
            //     _logger.LogError($"{c.ValueType}{c.Value}");
            // }
            if (permissions.Any())
            {
                _logger.LogInformation($"permissions.Any(): True");

                
                context.Succeed(requirement);
                await Task.CompletedTask;
                return;
            }

            _logger.LogError($"permissions.Any(): False");
        }
    
    }
}