using Api.Services.RabbitMq.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Calculation.Handlers
{
    public class CalculateSumHandler : IMessageHandler
    {
        public bool CanHandle(MessageType messageType)
        {
            return messageType == MessageType.CalculateSum;
        }

        public object Handle(object messageBody)
        {
            var list = JsonConvert.DeserializeObject<List<decimal>>(messageBody.ToString());

            return CalculateSum(list);
        }

        private static decimal CalculateSum(List<decimal> values)
        {
            if (values == null || !values.Any())
            {
                return 0;
            }

            return values.Sum();
        }

    }
}
