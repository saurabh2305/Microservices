using RabbitMQ.Client;
using System;
using System.Text;

namespace TopicPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "topic-exch",
                type: ExchangeType.Topic,
                durable: false,
                autoDelete: false,
                arguments: null);

            while (true)
            {
                Console.Write("Enter the routing key (<facility>.<severity>):");
                var routingkey = Console.ReadLine();
                Console.Write("Enter the message (Empty to Exit:");
                var message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    break;
                }
                var payload = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "topic-exch",
                    routingKey: routingkey,
                    mandatory: false,
                    basicProperties: null,
                    body: payload);
            }
        }
    }
}
