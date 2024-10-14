using System.Net;
using Application.IServices;
using Common.Requests.User;
using Common.Wrappers;
using MediatR;

namespace Application.Features.User;
public class DeleteUserCommand : IRequest<IResponseWrapper>
{
    public DeleteUserRequest _deleteUserRequest{get;set;}
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IResponseWrapper>
{
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<IResponseWrapper> Handle(DeleteUserCommand deleteUserCommand, CancellationToken cancellationToken)
        {
            return await _userService.DeleteUserAsync(deleteUserCommand._deleteUserRequest);
        }
}