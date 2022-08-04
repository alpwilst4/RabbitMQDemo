using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.WaterMark.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }
        public void Publish(productImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabbitMQClientService.Connect();
            var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var property = channel.CreateBasicProperties();
            property.Persistent = true;
            channel.BasicPublish(RabbitMQClientService.ExchangeName,RabbitMQClientService.RoutinWatermark,basicProperties:property,body:bodyByte);

             
        }
    }
}
