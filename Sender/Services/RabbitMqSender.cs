using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Sender.Services
{
    public class RabbitMqSender : IRabbitMqSender
    {
        public void MessageSend<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "rabbitmq_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish(exchange: "", routingKey: "rabbitmq_queue", body: body);
        }
    }
}
