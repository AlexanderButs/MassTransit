using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MassTransit;
using Protocol;

namespace Producer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.Message<Message>(c =>
                {
                    c.SetEntityName("Message");
                });
            });

            await bus.StartAsync();

            var tasks = new List<Task>();
            for (int i = 0; i < 4; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (true)
                    {
                        await bus.Publish(new Message{Text = "Hi"});
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}