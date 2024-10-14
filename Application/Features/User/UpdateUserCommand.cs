using Application.IServices;
using Common.Requests.User;
using Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User;
public class UpdateUserCommand : IRequest<IResponseWrapper>
{
    public UpdateUserRequest _updateUserRequest { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IResponseWrapper>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IResponseWrapper> Handle(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserAsync(updateUserCommand._updateUserRequest);
    }
}
