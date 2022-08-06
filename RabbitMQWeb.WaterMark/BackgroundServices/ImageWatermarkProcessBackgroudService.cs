
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQWeb.WaterMark.Services;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.WaterMark.BackgroundServices
{
    public class ImageWatermarkProcessBackgroudService : BackgroundService
    {

        private readonly RabbitMQClientService _rabbitMQClientService;
        private readonly ILogger<ImageWatermarkProcessBackgroudService> _logger;
        private IModel _channel;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public ImageWatermarkProcessBackgroudService(RabbitMQClientService rabbitMQClientService, ILogger<ImageWatermarkProcessBackgroudService> logger)
        {
            _channel = _rabbitMQClientService.Connect();
            _channel.BasicQos(0, 1, false);

            _rabbitMQClientService = rabbitMQClientService;
            _logger = logger;

            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMQClientService.QueueName,false,consumer);
            consumer.Received += Consumer_Recieved;

            return Task.CompletedTask;

        }
        private Task Consumer_Recieved(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var productImageCreatedEvent = JsonSerializer.Deserialize<productImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwwroot/images", productImageCreatedEvent.ImageName);
                var site = "www.mysite.com";

                using var img = Image.FromFile(path); //!!!!!!!
                using var graphic = Graphics.FromImage(img);
                var font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold, GraphicsUnit.Pixel);
                var textSize = graphic.MeasureString("www.mysite.com  ", font);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - (int)textSize.Height + 30);

                graphic.DrawString(site, font, brush, position);

                img.Save("wwwrott/Images/watrmarks/" + productImageCreatedEvent.ImageName);

                img.Dispose();
                graphic.Dispose();
                _channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception)
            {

                throw;
            }

            return Task.CompletedTask;
        }


        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
