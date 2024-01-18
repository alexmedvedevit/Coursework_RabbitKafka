using Confluent.Kafka;
using Receiver.Model;
using System.Text.Json;

namespace Receiver.Services
{
    public class KafkaListener : BackgroundService
    {
        const string topic = "kafka-contract";
        const string groupId = "groupId";
        const string bootstrapServers = "host.docker.internal:9092";

        ConsumerConfig config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        private readonly IServiceProvider _serviceProvider;

        public KafkaListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => Consume(stoppingToken));
            return Task.CompletedTask;
        }

        private async Task Consume(CancellationToken stoppingToken)
        {
            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume
                            (cancelToken.Token);
                        var cont = JsonSerializer.Deserialize<Contract>(consumer.Message.Value) ?? throw new Exception(); ;
                        using var scope = _serviceProvider.CreateScope();
                        var service = scope.ServiceProvider.GetRequiredService<IFinanceService>();
                        service.AllocateFunds(cont);
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
