using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RPCClinet
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpcClinet = new RPCClient();
            Console.WriteLine($" [x] Requesting fib({args[0]})");
            var response = rpcClinet.Call(args[0]);

            Console.WriteLine($" [.] Got '{response}'");
            rpcClinet.Close();
        }
    }
}
