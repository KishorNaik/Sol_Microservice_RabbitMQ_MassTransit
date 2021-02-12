using MassTransit;
using Shared.Domain.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Messages
{
    public class RequestResponseDemoConsumer : IConsumer<RequestDemoModel>
    {
        public async Task Consume(ConsumeContext<RequestDemoModel> context)
        {
            Console.WriteLine(context.Message.Id);

            // Send Dummy Record
            var responseDemoModel = new ResponseDemoModel()
            {
                Id = context.Message.Id,
                FullName = "Kishor Naik"
            };

            await context.RespondAsync<ResponseDemoModel>(responseDemoModel);
        }
    }
}