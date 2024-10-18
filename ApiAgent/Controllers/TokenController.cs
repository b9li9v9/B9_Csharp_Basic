using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common.Requests.Token;
using Common.ForwardMessage;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;
using Microsoft.Extensions.Logging;


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
            logger.LogInformation("receive");

            if (!await _producer.CreateModel(_producerConfiguration.ExchangeName,
                                            _producerConfiguration.ExchangeType,
                                            true, 
                                            _producerConfiguration.QueueName,
                                            true,
                                            false,
                                            false,
                                            _producerConfiguration.RoutingKey))
            {
                return BadRequest();
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            foreach(var h in HttpContext.Request.Headers)
            {
                headers.Add(h.Key,h.Value);
            }

            var httpRequestInfo = new ForwardRequestInfo(HttpContext.Request.Scheme, 
                                                            HttpContext.Request.QueryString, 
                                                            HttpContext.Request.Path, 
                                                            HttpContext.Request.Method, 
                                                            HttpContext.Request.Host, 
                                                            headers,
                                                            JObject.FromObject(createTokenRequest));
            var callBackPathInfo = new ForwardCallBackPath(_producerConfiguration.CallbackExchangeName,
                                                        _producerConfiguration.CallbackQueueName,
                                                        _producerConfiguration.CallbackRoutingKey);
            var forwardMessage = new ForwardMessage(Guid.NewGuid().ToString(),
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



            //return BadRequest();
        }

        //[MustHavePermissionAttribute(AppRoleGroup.BasicAccess,AppFeature.Users,AppAction.Update)]
        [HttpPost]
        public async Task<IActionResult> TestAsync()
        {
            return Ok("end");
        }
        
    }
}
