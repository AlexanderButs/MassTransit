using System;
using System.Threading.Tasks;

using GreenPipes;

using MassTransit;

namespace Consumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://localhost:5672/"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    //cfg.PrefetchCount = 32;
                    //cfg.UseConcurrencyLimit(10);

                    cfg.ReceiveEndpoint(host, e =>
                    {
                        e.Consumer<MessageConsumer>();
                    });
                });

                await busControl.StartAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}