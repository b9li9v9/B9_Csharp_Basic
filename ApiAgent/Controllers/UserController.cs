using Common.ForwardMessage;
using Common.Requests.Token;
using Common.Requests.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RabbitHelper.Configuration;
using RabbitHelper.IServices;

namespace ApiAgent.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        private readonly IProducer _producer;
        private readonly ProducerConfiguration _producerConfiguration;

        public UserController(IProducer producer, ProducerConfiguration producerConfiguration)
        {
            _producer = producer;
            _producerConfiguration = producerConfiguration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest createUserRequest)
        {

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserUserAsync(string id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchUserAsync(string id)
        {

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

            Dictionary<string, string> headers = new Dictionary<string, string>();
            foreach (var h in HttpContext.Request.Headers)
            {
                headers.Add(h.Key, h.Value);
            }

            var httpRequestInfo = new ForwardRequestInfo(HttpContext.Request.Scheme,
                                                            HttpContext.Request.QueryString,
                                                            HttpContext.Request.Path,
                                                            HttpContext.Request.Method,
                                                            HttpContext.Request.Host,
                                                            headers,
                                                            null);
            var callBackPathInfo = new ForwardCallBackPath(_producerConfiguration.CallbackExchangeName,
                                                           id,
                                                            id);
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

        }

    }
}
