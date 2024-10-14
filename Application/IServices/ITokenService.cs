using Common.Requests.Token;
using Common.Responses.Token;
using Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface ITokenService
    {
        public Task<ResponseWrapper<CreateTokenResponse>> CreateTokenAsync(CreateTokenRequest tokenRequest);
    }
}
