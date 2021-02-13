using Api.Services.RabbitMq.Contracts;

namespace Calculation.Handlers
{
    public class PingHandler : IMessageHandler
    {
        public bool CanHandle(MessageType messageType)
        {
            return messageType == MessageType.Ping;
        }

        public object Handle(object messageBody)
        {
            return "This is just a ping - have a nice day :)";
        }
    }
}
