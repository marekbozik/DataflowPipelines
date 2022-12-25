using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    public class Pipeline<TInput, TOutput>
    {

        public LinkedList<Node<TInput, TOutput>> Nodes { get; }

        public Pipeline()
        {
            Nodes = new LinkedList<Node<TInput, TOutput>>();
        }

        

        public void Compose(TInput input)
        {
            
        }

    }
}
