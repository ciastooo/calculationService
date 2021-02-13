namespace Api.Services.RabbitMq.Contracts
{
    public interface IMessageContract
    {
        MessageType MessageType { get; }

        object Data { get; }
    }

    public class MessageContract : IMessageContract
    {
        public MessageType MessageType { get; init; }

        public object Data { get; init; }

        private MessageContract()
        {
        }

        private MessageContract(MessageType messageType, object data)
        {
            MessageType = messageType;
            Data = data;
        }

        public static IMessageContract Create(MessageType messageType, object data)
        {
            return new MessageContract(messageType, data);
        }
    }

    public enum MessageType
    {
        CalculateSum = 0,
        CalculateAverage = 1,
    }
}
