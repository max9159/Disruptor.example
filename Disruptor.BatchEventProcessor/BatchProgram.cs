using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disruptor.basic;
using Disruptor.example;

namespace Disruptor.BatchEventProcessor
{
    class BatchProgram
    {
        private const int _ringSize = 16;  // Must be multiple of 2
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            var ringBuffer = RingBuffer<MyValueEntry>.CreateSingleProducer(() => new MyValueEntry(), _ringSize, new YieldingWaitStrategy());

            //Case A: 1 Producer - 1 Consumer (Unicast event)
            var sequenceBarrier = ringBuffer.NewBarrier();
            var batchProcessorPrintVal = new BatchEventProcessor<MyValueEntry>(ringBuffer, sequenceBarrier, new MyValueAdditionHandler());

            var processorTask1 = Task.Run(() => batchProcessorPrintVal.Run());
            batchProcessorPrintVal.WaitUntilStarted(TimeSpan.FromSeconds(5));

            Simulator.SimulatePublishEvent(ringBuffer, 1000);

            batchProcessorPrintVal.Halt();
            Task.WaitAll(processorTask1);

            sw.Stop();

            Console.WriteLine($"elapsed {sw.ElapsedMilliseconds} ms");
            Console.Read();
        }

    }
}
