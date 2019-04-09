﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disruptor.example;

namespace Disruptor.basic
{
    class Program
    {
        private static readonly Random _random = new Random();
        private const int _ringSize = 16;  // Must be multiple of 2
        static void Main(string[] args)
        {
            var disruptor = new Disruptor.Dsl.Disruptor<MyValueEntry>(() => new MyValueEntry(), _ringSize, System.Threading.Tasks.TaskScheduler.Default);

            //1 Producer - 2 Consumer (Steps Pipeline)
            disruptor.HandleEventsWith(
                new MyValueAdditionHandler()).Then(
                    new MyLogHandler(@"C:\Disruptor.basic.log"));
            ////1 Producer - 2 Consumer (multicast event)
            //disruptor.HandleEventsWith(
            //    new MyValueAdditionHandler(),
            //    new MyLogHandler(@"C:\Disruptor.basic.log"));

            var ringBuffer = disruptor.Start();

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

                Thread.Sleep(250);
            }
        }
    }
}
