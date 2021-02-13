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

        public MessageCoordinator(List<IMessageHandler> handlers)
        {
            this.handlers = handlers;
        }

        public object HandleMessage(IMessageContract message)
        {
            var handler = handlers.FirstOrDefault(m => m.CanHandle(message.MessageType));

            if(handler == null)
            {
                Console.WriteLine($"No handler found for {message.MessageType} messageType");
                return null;
            }

            return handler.Handle(message.Data);
        }

        public List<IMessageHandler> GetHandlers()
        {
            return handlers;
        }
    }
}
