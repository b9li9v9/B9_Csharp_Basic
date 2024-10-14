using Infrastructure.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Permissions
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        private readonly ILogger<PermissionPolicyProvider> _logger;
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options,ILogger<PermissionPolicyProvider> logger)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _logger = logger;
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(AppClaim.Permission, StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogInformation("policyName Build");
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }
            
            _logger.LogInformation("policyName error");
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        // public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        //     => FallbackPolicyProvider.GetDefaultPolicyAsync();


        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => Task.FromResult<AuthorizationPolicy>(null);
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
            => Task.FromResult<AuthorizationPolicy>(null);
    }
}
