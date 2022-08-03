using RabbitMQ.Client;

namespace RabbitMQWeb.WaterMark.Services
{
    public class RabbitMQClientService:IDisposable
    {
        private readonly ConnectionFactory? _connectionFactory;//bir kere set edilcek onun içi readnonly kullandım
        private IConnection? _connection;
        private IModel? _channel;
        public static string ExchangeName = "ImageDirectExhange";
        public static string RoutinWatermark = "ImageRouteWatermark";
        public static string QueueName = "ImageQueueWatermark";

        private readonly ILogger<RabbitMQClientService>? _logger;

        public RabbitMQClientService(ConnectionFactory? connectionFactory,ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }
        public IModel Connect()
        {
            _connection = _connectionFactory?.CreateConnection();
            if (_channel is { IsOpen:true})
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName,type:"direct",true,false);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(exchange: ExchangeName, queue: QueueName,routingKey:RoutinWatermark);

            _logger?.LogInformation("RabbitMQ bağlantısı kuruldu");

            return _channel;

        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger?.LogInformation("RabbitMQ bağlantısı Koptu");


        }
    }
}
