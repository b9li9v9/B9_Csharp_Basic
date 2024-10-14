using System.Reflection;
using Application.IServices;
using Azure.Core;
using Common.Requests.User;
using Common.Responses.User;
using Common.Wrappers;
using Infrastructure.Constant;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    internal class UserService : BaseService<UserService>, IUserService
    {
        public UserService(
            AppConfiguration appConfiguration, 
            ApplicationDbContext applicationDbContext, 
            ILogger<UserService> logger)
            : base(appConfiguration, applicationDbContext, logger){}

        public async Task<ResponseWrapper<CreateUserResponse>> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            _logger.LogInformation("CreateUserAsync work");
            if(_applicationDbContext.Users.Any(u => u.Email == createUserRequest.Email)) 
            {
                return await ResponseWrapper<CreateUserResponse>.FailAsync("Email already exists");
            }

            User newUser = new User{Id=Guid.NewGuid().ToString(),Name="",Email=createUserRequest.Email,Password=createUserRequest.Password};
            var basicRoleId = _applicationDbContext.Roles.FirstOrDefault(r=>r.NormalizedName=="basic".ToUpper());
            // _logger.LogInformation(role.Id)
            UserRole newUserRole = new UserRole{Id = Guid.NewGuid().ToString(),UserId=newUser.Id,RoleId=basicRoleId.Id};
            
            await _applicationDbContext.UserRoles.AddAsync(newUserRole);
            await _applicationDbContext.Users.AddAsync(newUser);
            await _applicationDbContext.SaveChangesAsync();    
            return await ResponseWrapper<CreateUserResponse>.SuccessAsync(
                new CreateUserResponse { Email = newUser.Email }, 
                $"Create {newUser.Email} Finish.");
        }

        public async Task<ResponseWrapper<DeleteUserResponse>> DeleteUserAsync(DeleteUserRequest deleteUserRequest)
        {
            // 删除请求成功后，这个token的exp时间内仍然可使用            
            // 所以每个非公开请求应该先验证UserTable是否存在这个user
            // 不存在就拒绝，不然这个token会漏

            var id = deleteUserRequest.Id;
            User currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(e=>e.Id ==id);
            
            if(currentUser is not null)
            {
                currentUser.IsDeleted = true;
                await _applicationDbContext.SaveChangesAsync();
                return await ResponseWrapper<DeleteUserResponse>.SuccessAsync(
                    new DeleteUserResponse { Id = currentUser.Id,Email=currentUser.Email }, 
                    $"delete {currentUser.Id} Finish.");
            }

            
            return await ResponseWrapper<DeleteUserResponse>.FailAsync("Invalid Id.");
        }

        public async Task<ResponseWrapper<SearchUserResponse>> SearchUserAsync(SearchUserRequest searchUserRequest)
        {
            
            var id = searchUserRequest.Id;
            User currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(e=>e.Id ==id);
            
            if(currentUser is not null)
            {
                return await ResponseWrapper<SearchUserResponse>.SuccessAsync(
                    new SearchUserResponse { Id=currentUser.Id,Email=currentUser.Email,Name=currentUser.Name}, 
                    $"SearchUser {currentUser.Id} Finish.");
            }

            
            return await ResponseWrapper<SearchUserResponse>.FailAsync("Invalid Id.");
        }

        public async Task<ResponseWrapper<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            var id = updateUserRequest.Id;
            User currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(e => e.Id == id);

            if (currentUser is not null)
            {
                currentUser.Name = updateUserRequest.Name;
                await _applicationDbContext.SaveChangesAsync();
                var updateUserResponse = await _applicationDbContext.Users.FirstOrDefaultAsync(e => e.Id == id);
                return await ResponseWrapper<UpdateUserResponse>.SuccessAsync(
                    new UpdateUserResponse { Id = updateUserResponse.Id, Email = updateUserResponse.Email, Name = updateUserResponse.Name },
                    $"UpdateUser {currentUser.Id} Finish.");
            }


            return await ResponseWrapper<UpdateUserResponse>.FailAsync("Invalid Id.");
        }
    }
}