using System.Net;
using Application.IServices;
using Common.Requests.User;
using Common.Wrappers;
using MediatR;

namespace Application.Features.User;
public class CreateUserCommand : IRequest<IResponseWrapper>
{
    public CreateUserRequest _createUserRequest{get;set;}
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResponseWrapper>
{
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<IResponseWrapper> Handle(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
        {
            return await _userService.CreateUserAsync(createUserCommand._createUserRequest);
        }
}