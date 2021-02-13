using Api.Services.RabbitMq.Contracts;

namespace Calculation.Handlers
{
    public interface IMessageHandler
    {
        bool CanHandle(MessageType messageType);

        object Handle(object messageBody);
    }

}
