// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;




var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-fanout",durable: true,type:ExchangeType.Fanout);


Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    string message = $"log {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);


    channel.BasicPublish("logs-fanout","", null, messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});


