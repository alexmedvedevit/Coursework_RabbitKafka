using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Sender.Model;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Sender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaContractController : Controller
    {
        private readonly string bootstrapServers = "host.docker.internal:9092";
        private readonly string topic = "kafka-contract";


        [HttpPost("api/kafka")]
        public async Task<IActionResult> Post(Contract request)
        {
            var message = JsonSerializer.Serialize(request);
            return Ok(await RequestSend(topic, message));
        }

        private async Task<bool> RequestSend(string topic, string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using var producer = new ProducerBuilder<Null, string>(config).Build();

                var result = await producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = message
                });

                Debug.WriteLine($"{result.Timestamp.UtcDateTime}");
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
