using System;

namespace Disruptor.basic
{
    public class MyValueAdditionHandler : IEventHandler<MyValueEntry>
    {
        public void OnEvent(MyValueEntry data, long sequence, bool endOfBatch)
        {
            Console.WriteLine("Event handled: Value = {0} (processed event {1}", data.Value, sequence);
        }
    }
}