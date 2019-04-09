using System;
using System.IO;
using Disruptor.basic;

namespace Disruptor.example
{
    public class MyLogHandler : IEventHandler<MyValueEntry>, IDisposable
    {
        private readonly StreamWriter _writer;
        public MyLogHandler(string path)
        {
            _writer = new StreamWriter(path, true);
        }

        public void OnEvent(MyValueEntry data, long sequence, bool endOfBatch)
        {
            AppendData(data);
            if (endOfBatch)
            {
                _writer.Flush();
            }
        }

        private void AppendData(MyValueEntry data)
        {
            _writer.Write($"{ DateTime.Now :O} : { data.Value }");
            _writer.Write(Environment.NewLine);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
