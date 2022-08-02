// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;






var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://fefpicsr:hi8rgpne3tFVJKaHs0hftBCnIx3mNfzh@cow.rmq2.cloudamqp.com/fefpicsr");


using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange",durable: true,type:ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

channel.BasicPublish("header-exchange", String.Empty, properties, Encoding.UTF8.GetBytes("header mesajım"));


Console.WriteLine("mesaj gönderilmiştir");

Console.ReadLine();


