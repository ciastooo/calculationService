using Calculation.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

namespace Calculation.Tests.Handlers
{
    [TestFixture]
    public class CalculateAverageHandlerTests
    {
        private IMessageHandler handler;

        [SetUp]
        public void Setup()
        {
            handler = new CalculateAverageHandler();
        }

        [Test]
        public void Handle_ReturnsCorrectAverage_ForListOfNumbers()
        {
            // Arrange
            var numbers = new List<decimal> { 0.5m, 1, 2, 3, 4, 4.5m };
            var expectedAverage = 15m/6;

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
