// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;




var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.QueueDeclare("hello-queue", true, false, false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    string message = $"Message {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);


    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody); // kuyrukraki isim verilmeli!!!!!!

    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});


