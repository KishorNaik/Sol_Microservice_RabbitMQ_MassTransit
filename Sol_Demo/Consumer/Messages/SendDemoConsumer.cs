using MassTransit;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Messages
{
    public class SendDemoConsumer : IConsumer<SendDemoModel>
    {
        public Task Consume(ConsumeContext<SendDemoModel> context)
        {
            Console.WriteLine(context.Message.Message);
            return Task.CompletedTask;
        }
    }
}