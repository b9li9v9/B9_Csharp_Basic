using System;
using System.Threading.Tasks;

namespace Common.ForwardMessage
{
    public class ForwardCallBackPath
    {
        public string callBackExchangeName { get; set; }
        public string callBackQueueName { get; set; }
        public string callBackRoutingKey { get; set; }


        public ForwardCallBackPath(string callbackExchangeName, string callbackQueueName, string callbackRoutingKey)
        {
            callBackExchangeName = callbackExchangeName;
            callBackQueueName = callbackQueueName;
            callBackRoutingKey = callbackRoutingKey;
        }
    }
}
