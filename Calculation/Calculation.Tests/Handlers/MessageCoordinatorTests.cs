using Api.Services.RabbitMq.Contracts;
using Calculation.Handlers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace Calculation.Tests.Handlers
{
    [TestFixture]
    public class MessageCoordinatorTests
    {
        private IMessageHandler mockedHandler;

        private MessageCoordinator messageCoordinator;

        [SetUp]
        public void Setup()
        {
            mockedHandler = Substitute.For<IMessageHandler>();

            messageCoordinator = new MessageCoordinator(new List<IMessageHandler> { mockedHandler });
        }

        [Test]
        public void HandleMessage_PassMessageToHandler_ForExistingMessageType()
        {
            // Arrange
            mockedHandler.CanHandle(Arg.Any<MessageType>()).ReturnsForAnyArgs(true);
            var message = CreateTestMessage();

            // Act
            messageCoordinator.HandleMessage(message);

            // Assert
            mockedHandler.Received(1).Handle(Arg.Is(message.Data));
        }

        [Test]
        public void HandleMessage_HandleMessage_ForExistingHandler()
        {
            // Arrange
            mockedHandler.CanHandle(Arg.Any<MessageType>()).ReturnsForAnyArgs(false);
            var message = CreateTestMessage();

            // Act
            messageCoordinator.HandleMessage(message);

            // Assert
            mockedHandler.Received(0).Handle(Arg.Is(message.Data));
        }

        private IMessageContract CreateTestMessage()
        {
            var messageData = new { someValue = 1 };
            return MessageContract.Create(MessageType.CalculateAverage, messageData);
        }
    }
}
