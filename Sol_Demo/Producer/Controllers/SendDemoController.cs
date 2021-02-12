using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Controllers
{
    [Route("api/send")]
    [ApiController]
    public class SendDemoController : ControllerBase
    {
        private readonly IBus bus = null;

        public SendDemoController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpPost("demo")]
        public async Task<IActionResult> Demo([FromBody] SendDemoModel sendDemoModel)
        {
            try
            {
                var endpoint = await bus.GetSendEndpoint(new Uri("queue:demo-send-queue"));
                await endpoint.Send<SendDemoModel>(sendDemoModel);
            }
            catch (Exception ex)
            {
            }

            return base.Ok();
        }
    }
}