// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;






var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-topic",durable: true,type:ExchangeType.Topic);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x => {
   
  

});

Random rnd = new Random();
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1, 5);


   

    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);


    string message = $"log-type {log1}-{log2}-{log3}";
    var routeKey = $"{log1}.{log2}.{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);



    channel.BasicPublish("logs-topic",routeKey, null, messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});


