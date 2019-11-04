using RabbitMQ.Client;
using System;
using System.Text;

namespace FanoutPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            // in fanout exchange type there is no binding key which bind queue to exchange
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //channel.ExchangeDeclare(exchange: "fanout-exch",  // Commented from fanout 
            //    type: ExchangeType.Fanout,
            //    durable: false,
            //    autoDelete: false,
            //    arguments: null);
            channel.ExchangeDeclare(exchange: "demo-exch",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null);

            while (true)
            {
                Console.Write("Enter the message (Empty to Exit:");
                var message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    break;
                }
                // setting basic properties
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.ContentType = "text/plain";
                props.Expiration = "1500";// set TTL per message
                var payload = Encoding.UTF8.GetBytes(message);
                //channel.BasicPublish(exchange: "fanout-exch",  // Commented from fanout 
                //    routingKey: "",
                //    mandatory: false,
                //    basicProperties: null,
                //    body: payload);

                channel.BasicPublish(exchange: "demo-exch",
                    routingKey: "",
                    mandatory: false,
                    basicProperties: props,
                    body: payload);
            }
        }
    }
}
