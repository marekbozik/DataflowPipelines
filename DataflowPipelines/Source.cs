using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    public class Source<TInput>
    {

        private Pipe<TInput> inputs;

        private Source()
        {
        }

        public static Source<TInput> CreateSource<T>(Node<TInput, T> node)
        {
            var source = new Source<TInput>();
            source.inputs = node.GetInput();
            return source;
        }

        public void Add(TInput input) => inputs.Write(input);
    }
}
