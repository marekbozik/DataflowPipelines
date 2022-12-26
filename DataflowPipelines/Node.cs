using System.Collections.Concurrent;

namespace DataflowPipelines
{

    public abstract class Node<TInput, TOutput> : INode
    {
        private Func<TInput, TOutput>? runFunction;


        private Pipe<TInput> inputs;
        private ConcurrentBag<Pipe<TOutput>> outputs;
        private TOutput lastOutput;

        protected Node()
        {
            inputs = new Pipe<TInput>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        protected Node(Func<TInput, TOutput> runFunction)
        {
            this.runFunction = runFunction;
            inputs = new Pipe<TInput>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        public static Node<TInput, TOutput> CreateNode(Func<TInput, TOutput> runFunction)
        {
            return new SimpleNode<TInput, TOutput>(runFunction);
        }

        public abstract TOutput Run(TInput input);
                
        /// <summary>
        /// Connects this node to destination node:
        /// (this_node)-[:OUTPUT]-[:INPUT]->(destination_node)
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <returns>Destination node</returns>
        public Node<TOutput, T> ConnectTo<T>(Node<TOutput, T> destinationNode)
        {
            destinationNode.inputs = this.GetOutput();
            return destinationNode;
        }

        public void AddToInput(TInput input) => inputs.Write(input);

        public Pipe<TInput> GetInput() => inputs;

        public Pipe<TOutput> GetOutput()
        {
            var pipe = new Pipe<TOutput>();
            outputs.Add(pipe);
            return pipe;
        }

        public void StartProcessing()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    TInput item = inputs.Read();
                    TOutput outputItem;
                    if (runFunction != null)
                        outputItem = runFunction(item);
                    else
                        outputItem = this.Run(item);

                    foreach (var pipe in outputs)
                    {
                        pipe.Write(outputItem);
                    }
                    lastOutput = outputItem;

                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}