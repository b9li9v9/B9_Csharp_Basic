namespace RabbitHelper.Configuration
{
    public class RabbitConfiguration
    {
        public string HostName { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Port { set; get; }
        public string QueueName { set; get; }
        public string ExchangeName { set; get; }
        public string RoutingKey { set; get; }
        public string ExchangeType { set; get; }
        public string CallbackExchangeName { set; get; }
        public string CallbackQueueName { set; get; }
        public string CallbackRoutingKey { set; get; }
    }
}
