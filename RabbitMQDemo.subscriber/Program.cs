// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;




var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();


var channel = connection.CreateModel();


channel.BasicQos(0, 1,false);
//channel.QueueDeclare("hello-queue", true, false, false);
//channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);
var randomQueueName = channel.QueueDeclare().QueueName;

channel.QueueBind(randomQueueName,"logs-fanout","",null);


//channel.QueueDeclare(randomQueueName, true, false, false); //fanout kalıcı hale getirme
var consumer = new EventingBasicConsumer(channel);
Console.WriteLine("loglar dinleniyor...");
channel.BasicConsume(randomQueueName, false, consumer);


consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Thread.Sleep(1500);
    Console.WriteLine("Gelen Mesaj:" + message);

    channel.BasicAck(args.DeliveryTag,false);
};

Console.ReadLine(); 