using Application.IServices;
using Common.Requests.User;
using Common.Wrappers;
using MediatR;

namespace Application.Features.User;
public class SearchUserCommand : IRequest<IResponseWrapper>
{
    public SearchUserRequest _searchUserRequest{get;set;}
}

public class SearchUserCommandHandler : IRequestHandler<SearchUserCommand, IResponseWrapper>
{
        private readonly IUserService _userService;

        public SearchUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<IResponseWrapper> Handle(SearchUserCommand searchUserCommand, CancellationToken cancellationToken)
        {
            return await _userService.SearchUserAsync(searchUserCommand._searchUserRequest);
        }
}