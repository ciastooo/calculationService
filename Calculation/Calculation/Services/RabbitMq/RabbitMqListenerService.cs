using Api.Services.RabbitMq.Contracts;
using Calculation.Handlers;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Api.Services.RabbitMq
{
    public interface IRabbitMqListenerService
    {
        void Listen();
    }

    public class RabbitMqListenerService : IRabbitMqListenerService, IDisposable
    {
        private readonly IRabbitMqConfig config;
        private readonly IMessageCoordinator messageCoordinator;

        private IConnection connection { get; set; }
        private IModel channel { get; set; }

        private string requestQueueName => $"{config.QueueName}_Request";
        private string responseQueueName => $"{config.QueueName}_Response";

        public RabbitMqListenerService(IRabbitMqConfig config, int retryCount, IMessageCoordinator messageCoordinator)
        {
            this.config = config;
            this.messageCoordinator = messageCoordinator;

            ConnectAndListen(retryCount);
        }

        private void ConnectAndListen(int retryCount = 10)
        {
            var factory = new ConnectionFactory
            {
                HostName = config.Hostname,
                UserName = config.UserName,
                Password = config.Password
            };

            var retries = retryCount;
            while (retries > 0)
            {
                retries--;
                try
                {
                    Console.WriteLine("Establishing connection with RabbitMq...");
                    connection = factory.CreateConnection();
                    channel = connection.CreateModel();
                    Console.WriteLine("Connected");
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine($"{retryCount-retries} Couldn't connect to RabbitMq, trying again in a few seconds...");
                    Thread.Sleep(3000);
                }
            }

            Console.WriteLine("Couldn't establish connection with RabbitMq");
        }

        public void Listen()
        {
            if(connection == null || !connection.IsOpen )
            {
                Console.WriteLine("No open connection with RabbitMq");
                return;
            }

            Console.WriteLine($"Lsitening to {requestQueueName}");
            channel.QueueDeclare(requestQueueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += HandleMessage;
            channel.BasicConsume(requestQueueName, true, consumer);

            Console.WriteLine("Listening for messages...");
        }

        private void HandleMessage(object sender, BasicDeliverEventArgs e)
        {
            var correlationId = e.BasicProperties.CorrelationId;
            try
            {
                var receivedMessage = MessageContract.Deserialize(e.Body.ToArray());

                Console.WriteLine($"Received {receivedMessage?.MessageType} message with CorrelationId {correlationId}");

                var responseValue = messageCoordinator.HandleMessage(receivedMessage);
                var responseMessage = MessageContract.Create(receivedMessage.MessageType, responseValue);

                PublishResponse(responseMessage, correlationId, responseQueueName);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occurred during handling of a message with a {correlationId} correlationId - {ex.Message}");
            }
        }

        private void PublishResponse(IMessageContract responseValue, string correlationId, string responseQueueName)
        {
            try
            {
                var body = responseValue.Serialize();

                var responseProps = channel.CreateBasicProperties();
                responseProps.CorrelationId = correlationId;

                channel.BasicPublish(string.Empty, responseQueueName, responseProps, body);

                Console.WriteLine($"Sending {responseValue?.MessageType} response with CorrelationId {correlationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during sending message with a {correlationId} correlationId - {ex.Message}");

            }
        }

        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}
