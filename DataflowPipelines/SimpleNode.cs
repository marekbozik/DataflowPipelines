using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    sealed class SimpleNode<TInput, TOutput> : Node<TInput, TOutput>
    {
        private Func<TInput, TOutput> runFunction;
        protected internal SimpleNode(Func<TInput, TOutput> runFunction) : base(runFunction)
        {
            if (runFunction == null) 
                throw new ArgumentNullException(nameof(runFunction));
            this.runFunction = runFunction;
        }

        public sealed override TOutput Run(TInput input)
        {
            return runFunction(input);
        }
    }
}
