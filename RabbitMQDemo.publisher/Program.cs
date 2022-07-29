// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;






var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-direct",durable: true,type:ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x => {

    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName,true,false,false);

    channel.QueueBind(queueName, "logs-direct",routeKey,null);

});


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1, 5);

    string message = $"log-type {log}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";


    channel.BasicPublish("logs-direct",routeKey, null, messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});


