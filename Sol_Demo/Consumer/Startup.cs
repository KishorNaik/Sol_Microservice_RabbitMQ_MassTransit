using Consumer.Messages;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit((config) =>
            {
                config.AddConsumer<SendDemoConsumer>(); // Send Consumer
                config.AddConsumer<PublishDemoConsumer>(); // Publish Consumer
                config.AddConsumer<RequestResponseDemoConsumer>(); // Request & Response Consumer.

                config.AddBus((busFactory) => Bus.Factory.CreateUsingRabbitMq((configRabbitMq) =>
                {
                    configRabbitMq.UseHealthCheck(busFactory);
                    configRabbitMq.Host(new Uri("rabbitmq://localhost"), (configHost) =>
                    {
                        configHost.Username("guest");
                        configHost.Password("guest");
                    }
                    );

                    // Send Config
                    configRabbitMq.ReceiveEndpoint("demo-send-queue", (configReceiveEndPoint) =>
                     {
                         configReceiveEndPoint.PrefetchCount = 16;
                         configReceiveEndPoint.UseMessageRetry(r => r.Interval(2, 100));
                         configReceiveEndPoint.ConfigureConsumer<SendDemoConsumer>(busFactory);
                     });

                    // Publish Config
                    configRabbitMq.ReceiveEndpoint("demo-publish-event", (configReceiveEndPoint) =>
                    {
                        configReceiveEndPoint.ConfigureConsumer<PublishDemoConsumer>(busFactory);
                    });

                    // Request & Response Config
                    configRabbitMq.ReceiveEndpoint("demo-request-response-queue", (configReceiveEndPoint) =>
                    {
                        configReceiveEndPoint.ConfigureConsumer<RequestResponseDemoConsumer>(busFactory);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Consumer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Consumer v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}