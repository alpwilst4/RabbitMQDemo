// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;




var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();


var channel = connection.CreateModel();


channel.BasicQos(0, 1,false);



var queueName= "direct-queue-Critical";




var consumer = new EventingBasicConsumer(channel);
Console.WriteLine("loglar dinleniyor...");
channel.BasicConsume(queueName, false, consumer);


consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Thread.Sleep(1500);
    Console.WriteLine("Gelen Mesaj:" + message);

   // File.AppendAllText("log-crytical.txt",message+"\n", Encoding.UTF8); // dosyaya yazdırır


    channel.BasicAck(args.DeliveryTag,false); //doğrulama ,ç,n
};

Console.ReadLine(); 