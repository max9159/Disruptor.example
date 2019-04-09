using System;

namespace Disruptor.basic
{
    public class MyValueAdditionHandler : IEventHandler<MyValueEntry>
    {
        public void OnEvent(MyValueEntry data, long sequence, bool endOfBatch)
        {
            Console.WriteLine($"[MyValueAdditionHandler] OnEvent: Value = { data.Value } (processed event sequence { sequence }");
        }
    }
}