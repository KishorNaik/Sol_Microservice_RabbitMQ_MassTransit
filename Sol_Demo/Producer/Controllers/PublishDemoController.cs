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
    [Route("api/publish")]
    [ApiController]
    public class PublishDemoController : ControllerBase
    {
        private readonly IBus bus = null;

        public PublishDemoController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpPost("demo")]
        public async Task<IActionResult> Demo([FromBody] PublishDemoModel publishDemoModel)
        {
            try
            {
                await bus.Publish<Shared.Domain.PublishDemoModel>(publishDemoModel);
            }
            catch (Exception ex)
            {
            }

            return base.Ok();
        }
    }
}