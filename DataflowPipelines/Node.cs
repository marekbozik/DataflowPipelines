using System.Collections.Concurrent;

namespace DataflowPipelines
{
    public abstract class Node<TInput, TOutput> : INode
    {
        private Func<TInput, TOutput>? runFunction;


        //private Pipe<TInput> inputs;
        //private ConcurrentBag<Pipe<TOutput>> outputs;

        private BlockingCollection<TInput> inputs;
        private BlockingCollection<TOutput> outputs;

        public Node(Func<TInput, TOutput> runFunction)
        {
            this.runFunction = runFunction;
            inputs = new BlockingCollection<TInput>();
            outputs = new BlockingCollection<TOutput>();
        }

        public Node<TInput, TOutput> CreateNode(Func<TInput, TOutput> runFunction)
        {
            return new SimpleNode<TInput, TOutput>(runFunction);
        }

        public abstract TOutput Run(TInput input);

        private void SetInput(Node<TInput, TOutput> soureNode)
        {
            this.inputs = (BlockingCollection<TInput>)Convert.ChangeType(soureNode.outputs, typeof(BlockingCollection<TInput>));
        }


        public void Connect(Node<TInput, TOutput> soureNode)
        {
            SetInput(soureNode);
        }
        public void AddToInput(TInput input) => inputs.Add(input);

        public BlockingCollection<TOutput> GetOutput() => outputs;

        public void StartProcessing()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    TInput item = inputs.Take();
                    if (runFunction != null)
                        outputs.Add(runFunction(item));
                    else
                        outputs.Add(this.Run(item));

                }
            }, TaskCreationOptions.LongRunning);
        }

    }
}