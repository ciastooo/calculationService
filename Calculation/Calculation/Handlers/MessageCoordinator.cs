using Api.Services.RabbitMq.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculation.Handlers
{
    public interface IMessageCoordinator
    {
        object HandleMessage(IMessageContract message);
    }

    public class MessageCoordinator : IMessageCoordinator
    {
        private readonly List<IMessageHandler> handlers;

        public MessageCoordinator()
        {
            handlers = new List<IMessageHandler>();
            handlers.Add(new CalculateAverageHandler());
            handlers.Add(new CalculateSumHandler());
        }

        public object HandleMessage(IMessageContract message)
        {
            var handler = handlers.Single(m => m.CanHandle(message.MessageType));

            if(handler == null)
            {
                Console.WriteLine($"No handler found for {message.MessageType} messageType");
            }

            return handler.Handle(message.Data);
        }
    }
}
