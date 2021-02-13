using Api.Services.RabbitMq.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Calculation.Handlers
{
    public class CalculateAverageHandler : IMessageHandler
    {
        public bool CanHandle(MessageType messageType)
        {
            return messageType == MessageType.CalculateAverage;
        }

        public object Handle(object messageBody)
        {
            var list = JsonConvert.DeserializeObject<List<decimal>>(messageBody.ToString());

            return CalculateAverage(list);
        }

        private static decimal CalculateAverage(List<decimal> values)
        {
            if (values == null || !values.Any())
            {
                return 0;
            }

            return values.Average();
        }

    }
}
