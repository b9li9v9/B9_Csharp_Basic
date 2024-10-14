using System.Net;
using Application.IServices;
using Common.Requests.Token;
using Common.Wrappers;
using MediatR;

namespace Application.Features.Token;
public class CreateTokenCommand : IRequest<IResponseWrapper>
{
    public CreateTokenRequest _createTokenRequest{get;set;}
}

public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, IResponseWrapper>
{
        private readonly ITokenService _tokenService;

        public CreateTokenCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        
        public async Task<IResponseWrapper> Handle(CreateTokenCommand createTokenCommand, CancellationToken cancellationToken)
        {
            return await _tokenService.CreateTokenAsync(createTokenCommand._createTokenRequest);
        }
}