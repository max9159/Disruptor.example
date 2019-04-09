using System;

namespace Disruptor.basic
{
    public sealed class MyValueEntry
    {
        public long Value { get; set; }

        public MyValueEntry()
        {
            Console.WriteLine("New ValueEntry created");
        }
    }
}