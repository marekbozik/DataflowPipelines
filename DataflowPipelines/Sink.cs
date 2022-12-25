using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    public class Sink<T>
    {

        private BlockingCollection<T> outputs;

        public Sink(Node<T, T> node)
        {
            outputs = node.GetOutput();
        }

        public bool TryGet(out T item)
        {
            return outputs.TryTake(out item);
        }

        public T Get()
        {
            return outputs.Take();
        }
    }
}
