using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common.Requests.Token;
using Common.ForwardMessage;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;


namespace ApiAgent.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TokenController : BaseController<TokenController>
    {
        private readonly IProducer _producer;
        private readonly ProducerConfiguration _producerConfiguration;

        public TokenController(IProducer producer, ProducerConfiguration producerConfiguration)
        {
            _producer = producer;
            _producerConfiguration = producerConfiguration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenAsync([FromBody] CreateTokenRequest createTokenRequest)
        {
            //var ContentTypeValue = HttpContext.Request.Headers["Content-Type"];
            //return Ok(AuthorizationValue);

            if (!await _producer.CreateModel(_producerConfiguration.ExchangeName,
                                            _producerConfiguration.ExchangeType,
                                            true, _producerConfiguration.QueueName,
                                            true,
                                            false,
                                            false,
                                            _producerConfiguration.RoutingKey))
            {
                return BadRequest();
            }
            var httpContextRequest = HttpContext.Request;

            var requestURL = httpContextRequest.Path;
            var requestMethod = httpContextRequest.Method;
            var requestHeaders = httpContextRequest.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var requestBody = createTokenRequest;

            var httpRequestInfo = new ForwardRequestInfo(requestURL,
                                                                            requestMethod,
                                                                            requestHeaders,
                                                                            JObject.FromObject(requestBody));
            var callBackPathInfo = new ForwardCallBackPath(_producerConfiguration.CallbackExchangeName, 
                                                        _producerConfiguration.CallbackQueueName, 
                                                        _producerConfiguration.CallbackRoutingKey);
            var forwardMessage = new ForwardMessage(Guid.NewGuid().ToString(),
                                                           "CreateTokenRequest",
                                                              httpRequestInfo,
                                                              callBackPathInfo);
            string serializedforwardMessage = JsonConvert.SerializeObject(forwardMessage);
            bool send = await _producer.SendMessage(serializedforwardMessage,
                                                        _producerConfiguration.RoutingKey,
                                                        _producerConfiguration.ExchangeName);

            if (send)
            {
                return Ok(send);
            }
            return BadRequest(send);
        }

        //[MustHavePermissionAttribute(AppRoleGroup.BasicAccess,AppFeature.Users,AppAction.Update)]
        [HttpPost]
        public async Task<IActionResult> TestAsync()
        {
            return Ok("end");
        }
        
    }
}
