using RabbitMQ.Client;

namespace RabbitHelper.IServices
{
    public interface IProducer
    {
        public Task<bool> CreateModel(string exchange,
                                 string type,
                                 bool exchangeDurable,

                                 string queue,
                                 bool queueDurable,
                                 bool exclusive,
                                 bool autoDelete,

                                 string routingKey,
                                 IDictionary<string, object> arguments = null);
        public Task<bool> SendMessage(string _message,
                                string _routingKey,
                                string _exchange,
                                IBasicProperties _basicProperties = null);
        void Dispose();
    }
}
