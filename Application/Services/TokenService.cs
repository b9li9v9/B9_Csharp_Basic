using Application.IServices;
using Common.Requests.Token;
using Common.Responses.Token;
using Common.Wrappers;
using Infrastructure.Constant;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class TokenService : ITokenService
    {
        private readonly AppConfiguration _appConfiguration;
        private readonly ApplicationDbContext _applicationDbContext;

        public TokenService(AppConfiguration appConfiguration, ApplicationDbContext applicationDbContext)
        {
            _appConfiguration = appConfiguration;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ResponseWrapper<CreateTokenResponse>> CreateTokenAsync(CreateTokenRequest createTokenRequest)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == createTokenRequest.Email);
            if (user == null)
            { 
                return await ResponseWrapper<CreateTokenResponse>.FailAsync("Invalid Credentials."); 
            }
            if (user.Password != createTokenRequest.Password)
            {
                return await ResponseWrapper<CreateTokenResponse>.FailAsync("Invalid Credentials.");
            }

            var token = await GenerateJWTAsync(user);
            // return
            var response = new CreateTokenResponse
            {
                Token = token,
            };

            return await ResponseWrapper<CreateTokenResponse>.SuccessAsync(response,$"Hi! {user.Email}");

        }

        private async Task<string> GenerateJWTAsync(User user)
        {
            var token = GenerateEncrytedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private string GenerateEncrytedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_appConfiguration.TokenExpiryInMinutes),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfiguration.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            // 拿到用户身份集合
            IEnumerable<UserRole> userRoles = await _applicationDbContext.UserRoles.Where(r => r.UserId == user.Id).ToListAsync();
            //IEnumerable<RoleClaim> roleClaims = await _applicationDbContext.RoleClaims.Where(r => r.UserId == user.Id).ToListAsync();

            var tokenUserRoles = new List<Claim>();
            var tokenRoleClaims = new List<Claim>();

            foreach (var userRole in userRoles)
            {
                // 用户身份集合里取出身份集合
                IEnumerable<Role> roles = await _applicationDbContext.Roles.Where(r => r.Id == userRole.RoleId).ToListAsync();
                foreach (var role in roles) 
                {
                    // 身份集合里取出身份
                    tokenUserRoles.Add(new Claim(AppJwtPayloadTypes.Roles, role.Name));
                    // 身份里取出证明
                    IEnumerable<RoleClaim> roleClaims = await _applicationDbContext.RoleClaims.Where(r => r.RoleId == role.Id.ToString()).ToListAsync();
                    foreach (var roleClaim in roleClaims)
                    {
                        tokenRoleClaims.Add(new Claim(AppJwtPayloadTypes.Permission, roleClaim.ClaimValue));
                    }
                }
            }

            var tokenUser = new List<Claim>
            {
                new(AppJwtPayloadTypes.UserId, user.Id),
                new(AppJwtPayloadTypes.UserEmail, user.Email),
            }
            .Union(tokenUserRoles)
            .Union(tokenRoleClaims);

            return tokenUser;
        }
    }
}
