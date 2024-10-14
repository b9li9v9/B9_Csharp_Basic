using Common.Requests.User;
using Common.Responses.User;
using Common.Wrappers;

namespace Application.IServices;
//Update Search Delete Create
public interface IUserService
{
    public Task<ResponseWrapper<CreateUserResponse>> CreateUserAsync(CreateUserRequest createUserRequest);
    
    public Task<ResponseWrapper<DeleteUserResponse>> DeleteUserAsync(DeleteUserRequest deleteUserRequest);
    
    public Task<ResponseWrapper<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest);
    
    public Task<ResponseWrapper<SearchUserResponse>> SearchUserAsync(SearchUserRequest searchUserRequest);
}