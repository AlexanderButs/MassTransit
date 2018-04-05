using System;
using System.Threading;
using System.Threading.Tasks;

using MassTransit;

using Protocol;

namespace Consumer
{
    public class MessageConsumer : IConsumer<Message>
    {
        private static long _start = DateTime.UtcNow.Ticks;
        private static long _output = DateTime.UtcNow.Ticks;
        private static long _counter = 0;

        public async Task Consume(ConsumeContext<Message> context)
        {
            /*long counter = Interlocked.Read(ref _counter);
            if (counter == 0)
            {
                Interlocked.Exchange(ref _start, DateTime.UtcNow.Ticks);
            }*/

            var counter = Interlocked.Increment(ref _counter);

            long output = Interlocked.Read(ref _output);
            if (DateTime.UtcNow.Ticks - output >= 10_000_000)
            {
                Interlocked.Exchange(ref _output, DateTime.UtcNow.Ticks);

                var time = (DateTime.UtcNow.Ticks - _start) / 10_000_000;
                var speed = time != 0 ? counter / time : 0;
                Console.WriteLine($"{speed} msg/sec");
            }

            //Console.WriteLine(context.Message.Text);

            await Task.Yield();
        }
    }
}