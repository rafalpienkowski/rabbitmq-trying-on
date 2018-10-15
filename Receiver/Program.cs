using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory(){ HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    var queueName = channel.QueueDeclare().QueueName;

                    // topic
                    if(args.Length < 1)
                    {
                        Console.Error.WriteLine("Usage: {0} [binding_key...]", Environment.GetCommandLineArgs()[0]);
                            Console.WriteLine(" Press [enter] to exit.");
                            Environment.ExitCode = 1;
                            return;
                    }
                    
                    // fanout
                    // channel.QueueBind(
                    //     queue: queueName,
                    //     exchange: "logs",
                    //     routingKey: ""
                    // );

                    // direct
                    // foreach(var severity in args)
                    // {
                    //     channel.QueueBind(
                    //         queue: queueName,
                    //         exchange: "direct_logs",
                    //         routingKey: severity
                    //     );
                    // }

                    // Console.WriteLine($"Waiting for logs in queue: {queueName}");

                    foreach(var bindingKey in args)
                    {
                        channel.QueueBind(queue: queueName,
                                  exchange: "topic_logs",
                                  routingKey: bindingKey);
                    }

                    Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) => 
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        //Console.WriteLine($" [x] {message}");
                        Console.WriteLine($" [x] {routingKey}:{message}");
                    };

                    channel.BasicConsume(
                        queue: queueName,
                        autoAck: true,
                        consumer: consumer
                    );

                    Console.WriteLine("Press [enter] to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
