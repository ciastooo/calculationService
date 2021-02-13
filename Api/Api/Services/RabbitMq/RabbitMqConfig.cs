using Microsoft.Extensions.Options;

namespace Api.Services.RabbitMq
{
    public interface IRabbitMqConfig
    {
        string Hostname { get; }
        string UserName { get; }
        string Password { get; }
        string QueueName { get; }
    }

    public class RabbitMqConfig : IRabbitMqConfig
    {
        public string Hostname { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
        public string QueueName { get; init; }

        public RabbitMqConfig()
        {

        }

        public RabbitMqConfig(IOptions<RabbitMqConfig> config)
        {
            Hostname = config.Value.Hostname;
            UserName = config.Value.UserName;
            Password = config.Value.Password;
            QueueName = config.Value.QueueName;
        }
    }
}
