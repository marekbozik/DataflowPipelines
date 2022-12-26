using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    public class Sink<TOutput>
    {

        private Pipe<TOutput> outputs;

        private Sink()
        {
        }

        public static Sink<TOutput> CreateSink<T>(Node<T, TOutput> node)
        {
            var sink = new Sink<TOutput>();
            sink.outputs = node.GetOutput();
            return sink;
        }

        public TOutput Get()
        {
            return outputs.Read();
        }
    }
}
