using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    sealed class SimpleNode<TInput, TOutput> : Node<TInput, TOutput>
    {
        protected internal SimpleNode(Func<TInput, TOutput> runFunction) : base(runFunction)
        {
        }

        public sealed override TOutput Run(TInput input)
        {
            throw new NotImplementedException();
        }
    }
}
