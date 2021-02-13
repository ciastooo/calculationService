using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Api.Services.RabbitMq.Contracts
{
    public interface IMessageContract
    {
        MessageType MessageType { get; }

        object Data { get; }

        byte[] Serialize();
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

        public static IMessageContract Deserialize(byte[] data)
        {
            return Deserialize(Encoding.UTF8.GetString(data));
        }

        public static IMessageContract Deserialize(string json)
        {
            var deserializedMessage = JsonConvert.DeserializeObject<JObject>(json);

            var messageType = deserializedMessage.GetValue(nameof(MessageType)).ToObject<MessageType>();
            var data = deserializedMessage.GetValue(nameof(Data)).ToObject<object>();

            return new MessageContract(messageType, data);
        }

        public byte[] Serialize()
        {
            var json = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(json);
        }
    }

    public enum MessageType
    {
        CalculateSum = 0,
        CalculateAverage = 1,
    }
}
