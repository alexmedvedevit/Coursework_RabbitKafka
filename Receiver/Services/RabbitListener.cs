using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Diagnostics;
using System.Text.Json;
using Receiver.Model;

namespace Receiver.Services
{
    public class RabbitListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitListener(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory { HostName = "host.docker.internal" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "rabbitmq_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Debug.WriteLine($"Получено сообщение: {content}");
                var cont = JsonSerializer.Deserialize<Contract>(content) ?? throw new Exception();
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IFinanceService>();
                service.AllocateFunds(cont);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("rabbitmq_queue", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
