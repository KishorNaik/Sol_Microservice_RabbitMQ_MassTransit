using MassTransit;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Messages
{
    public class PublishDemoConsumer : IConsumer<PublishDemoModel>
    {
        public Task Consume(ConsumeContext<PublishDemoModel> context)
        {
            Console.WriteLine(context.Message.Message);
            return Task.CompletedTask;
        }
    }
}