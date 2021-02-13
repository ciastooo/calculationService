using Api.Services.RabbitMq;
using Calculation.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return new RabbitMqListenerService(config, retries, CreateMessageCoordinator());
        }

        private static IMessageCoordinator CreateMessageCoordinator()
        {
            var handlerInterface = typeof(IMessageHandler);
            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsInterface)
                .Where(p => handlerInterface.IsAssignableFrom(p))
                .ToList();

            var handlers = new List<IMessageHandler>();

            foreach (var handlerType in handlerTypes)
            {
                var handler = Activator.CreateInstance(handlerType);
                handlers.Add((IMessageHandler)handler);
            }

            return new MessageCoordinator(handlers);
        }
    }
}
