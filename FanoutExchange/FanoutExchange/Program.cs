using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FanoutExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Queuename:"  + args[0]);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();      

            channel.ExchangeDeclare(exchange: "fanout-exch",
              type: ExchangeType.Fanout,
              durable: true,
              autoDelete: false,
              arguments: null);

            var arguments = new Dictionary<string, object>()
            {
                { "x-message-ttl",60000 },  // ttl for queue messages           // added new lines for advanced
                { "x-expires",30*60*1000 }  // Queue expiry after idle timeout   // added new lines for advanced
            };
            channel.QueueDeclare("demoq", durable: false, exclusive: false, autoDelete: false);
            channel.QueueBind("demoq", "fanout-exch", "", arguments:arguments);

            //channel.QueueDeclare(args[0], durable: false, exclusive: false, autoDelete: false); // commented from fanout exchange
            //channel.QueueBind(args[0], "fanout-exch", "", null); // commented from fanout exchange

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                Console.WriteLine($"Message Received:{message}");
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };
            // channel.BasicConsume(args[0], true, consumer); commented from fanout exchange
             channel.BasicConsume(queue:"demoq",autoAck:false, consumer: consumer);
            Console.WriteLine("Waiting for messages to enter--- press enter to exit");
            Console.ReadLine();
        }
    }
}
