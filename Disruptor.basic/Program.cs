using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disruptor.example;

namespace Disruptor.basic
{
    class Program
    {
        private const int _ringSize = 16;  // Must be multiple of 2
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            var disruptor = new Disruptor.Dsl.Disruptor<MyValueEntry>(() => new MyValueEntry(), _ringSize, System.Threading.Tasks.TaskScheduler.Default);

            //Case A: 1 Producer - 2 Consumer (Steps Pipeline)
            disruptor.HandleEventsWith(
                new MyValueAdditionHandler()).Then(
                    new MyLogHandler(@"C:\Disruptor.basic.log"));

            ////Case B: 1 Producer - 2 Consumer (multicast event)
            //disruptor.HandleEventsWith(
            //    new MyValueAdditionHandler(),
            //    new MyLogHandler(@"C:\Disruptor.basic.log"));

            var ringBuffer = disruptor.Start();

            Simulator.SimulatePublishEvent(ringBuffer, 1000);

            sw.Stop();

            Console.WriteLine($"elapsed {sw.ElapsedMilliseconds} ms");
            Console.Read();

        }
    }
}
