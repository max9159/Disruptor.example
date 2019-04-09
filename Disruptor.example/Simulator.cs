using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor.basic;

namespace Disruptor.example
{
    public class Simulator
    {
        private static readonly Random _random = new Random();
        public static void SimulatePublishEvent(RingBuffer<MyValueEntry> ringBuffer, long simulateCount)
        {
            while (true)
            {
                long sequenceNo = ringBuffer.Next();
                MyValueEntry entry;
                try
                {
                    entry = ringBuffer[sequenceNo];
                    entry.Value = _random.Next();
                }
                finally
                {
                    //make sure the event will be sent.
                    ringBuffer.Publish(sequenceNo);
                }

                Console.WriteLine("Published entry {0}, value {1}", sequenceNo, entry.Value);
                if (sequenceNo == simulateCount)
                    break;
            }
        }
    }
}
