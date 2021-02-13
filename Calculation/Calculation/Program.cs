using Api.Services.RabbitMq;
using Calculation.Handlers;
using System;
using System.Threading;

namespace Calculation
{
    class Program
    {
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Starting calculation service...");

            var config = new RabbitMqConfig
            {
                Hostname = "rabbitmq",
                Password = "password",
                UserName = "user",
                QueueName = "Queue"
            };
            var retries = 10;

            var listener = InitializeListenerService(config, retries);
            listener.Listen();

            waitHandle.WaitOne();
        }

        private static IRabbitMqListenerService InitializeListenerService(IRabbitMqConfig config, int retries)
        {
            var messageCoordinator = new MessageCoordinator();
            return new RabbitMqListenerService(config, retries, messageCoordinator);
        }
    }
}
