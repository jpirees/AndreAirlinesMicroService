using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace LogProducer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsProducerController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private const string QUEUE_NAME = "messagelogs";

        public LogsProducerController()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] Log message)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var stringfieldMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage
                        );
                }
            }
            return Accepted();
        }
    }
}
