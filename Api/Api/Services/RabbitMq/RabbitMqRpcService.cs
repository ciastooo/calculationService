using Api.Services.RabbitMq.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services.RabbitMq
{
    public interface IRabbitMqRpcService
    {
        Task<TResponse> Send<TResponse>(MessageType messageType, object data);
    }

    public class RabbitMqRpcService : IRabbitMqRpcService, IDisposable
    {
        private readonly IRabbitMqConfig config;
        private readonly ILogger<RabbitMqRpcService> logger;

        private IConnection connection { get; set; }
        private IModel channel { get; set; }
        private EventingBasicConsumer consumer { get; set; }

        private ConcurrentDictionary<Guid, TaskCompletionSource<IMessageContract>> pendingMessages;

        private string requestQueueName => $"{config.QueueName}_Request";
        private string responseQueueName => $"{config.QueueName}_Response";

        public RabbitMqRpcService(IRabbitMqConfig config, ILogger<RabbitMqRpcService> logger)
        {
            this.config = config;
            this.logger = logger;
            pendingMessages = new ConcurrentDictionary<Guid, TaskCompletionSource<IMessageContract>>();

            ConnectAndListen();
        }
        
        public Task<TResponse> Send<TResponse>(MessageType messageType, object data)
        {
            var message = MessageContract.Create(messageType, data);

            var taskCompletionSource = new TaskCompletionSource<IMessageContract>();
            var correlationId = Guid.NewGuid();

            pendingMessages.TryAdd(correlationId, taskCompletionSource);

            Publish(message, correlationId);

            return taskCompletionSource.Task.ContinueWith(t => (TResponse)t.Result);
        }

        private void ConnectAndListen()
        {
            var factory = new ConnectionFactory
            {
                HostName = config.Hostname,
                UserName = config.UserName,
                Password = config.Password
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(requestQueueName, true, false, false, null);
            channel.QueueDeclare(responseQueueName, true, false, false, null);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += HandleResponse;
            channel.BasicConsume(responseQueueName, true, consumer);
        }

        private void HandleResponse(object sender, BasicDeliverEventArgs e)
        {
            var content = Encoding.UTF8.GetString(e.Body.ToArray());
            var responseValue = JsonConvert.DeserializeObject<IMessageContract>(content);
            var correlationId = e.BasicProperties.CorrelationId;

            logger.LogInformation($"Received {responseValue?.MessageType} message with CorrelationId {correlationId}");

            if (pendingMessages.TryRemove(Guid.Parse(correlationId), out var taskCompletionSource))
            {
                taskCompletionSource.SetResult(responseValue);
            }
        }

        private void Publish(IMessageContract message, Guid correlationId)
        {
            var props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId.ToString();
            props.ReplyTo = responseQueueName;

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            logger.LogInformation($"Sending {message?.MessageType} message with CorrelationId {correlationId}");
            channel.BasicPublish(string.Empty, requestQueueName, props, body);
        }

        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}
