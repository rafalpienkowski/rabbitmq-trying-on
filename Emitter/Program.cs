using System;
using RabbitMQ.Client;
using System.Text;
using System.Linq;

namespace Emitter
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    
                    // channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    // Change from fanout to direct type
                    // channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                    // Topic
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                    var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";

                    var message = GetMessage(args.Skip(1).ToArray());
                    var body = Encoding.UTF8.GetBytes(message);

                    // Exchange
                    // channel.BasicPublish(
                    //     exchange: "logs",
                    //     routingKey: "",
                    //     basicProperties: null,
                    //     body: body
                    // );

                    // Direct
                    // var serverities = new[] {"critical", "log", "warn"};
                    // foreach(var serverity in serverities)
                    // {
                    //     channel.BasicPublish(
                    //         exchange: "direct_logs",
                    //         routingKey: serverity,
                    //         basicProperties: null,
                    //         body: body
                    //     );
                    // }

                    // Topic

                    channel.BasicPublish(
                        exchange: "topic_logs",
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body
                    );

                    //Console.WriteLine($" [x] Sent {message}");
                    Console.WriteLine($" [x] Sent {routingKey}:{message}");
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
