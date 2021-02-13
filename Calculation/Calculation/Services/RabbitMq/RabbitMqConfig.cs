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
    }
}
