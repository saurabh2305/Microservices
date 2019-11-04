using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "csq", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
             {
                 var message = Encoding.UTF8.GetString(ea.Body);
                 Console.WriteLine($"Message Received:{message}");
             };
            channel.BasicConsume(queue: "csq", autoAck: true, consumer: consumer);
            Console.WriteLine("Waiting for messages..press enter to exit");
            Console.ReadLine();
            channel.Dispose();
            connection.Dispose();
        }
    }
}
