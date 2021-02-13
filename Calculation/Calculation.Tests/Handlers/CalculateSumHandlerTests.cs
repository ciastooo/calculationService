using Calculation.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

namespace Calculation.Tests.Handlers
{
    [TestFixture]
    public class CalculateSumHandlerTests
    {
        private IMessageHandler handler;

        [SetUp]
        public void Setup()
        {
            handler = new CalculateSumHandler();
        }

        [Test]
        public void Handle_ReturnsCorrectSum_ForListOfNumbers()
        {
            // Arrange
            var numbers = new List<decimal> { 1, 2, 3, 4, 1.123456789m, 8.876543211m };
            var expectedAverage = 20;

            // Act
            var result = handler.Handle(CreateMessageBody(numbers));

            // Assert
            Assert.That(result, Is.EqualTo(expectedAverage));
        }

        [Test]
        public void Handle_ReturnsZero_ForEmptyList()
        {
            // Arrange
            var numbers = new List<decimal>();

            // Act
            var result = handler.Handle(CreateMessageBody(numbers));

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        private string CreateMessageBody(List<decimal> values)
        {
            return JsonConvert.SerializeObject(values);
        }
    }
}
