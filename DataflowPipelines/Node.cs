using System.Collections.Concurrent;

namespace DataflowPipelines
{

    public abstract class Node<TInput, TOutput> : INode
    {
        private Func<TInput, TOutput>? runFunction;
        public Pipe<TInput> Input { get; set; } 
        private ConcurrentBag<Pipe<TOutput>> outputs;
        private TOutput lastOutput;

        protected Node()
        {
            Input = new Pipe<TInput>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        protected Node(Func<TInput, TOutput> runFunction)
        {
            this.runFunction = runFunction;
            Input = new Pipe<TInput>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        public static Node<TInput, TOutput> CreateNode(Func<TInput, TOutput> runFunction)
        {
            return new SimpleNode<TInput, TOutput>(runFunction);
        }

        /// <summary>
        /// This method implements node application logic
        /// </summary>
        /// <remarks>This method must be implemented in derived classes</remarks>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract TOutput Run(TInput input);

        /// <summary>
        /// Connects this node to destination node:
        /// (this_node)-[:OUTPUT]-[:INPUT]->(destination_node)
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <remarks>This method can be chained</remarks>
        /// <returns>Destination node</returns>
        public Node<TOutput, T> ConnectTo<T>(Node<TOutput, T> destinationNode)
        {
            this.GetOutput().StreamTo(destinationNode.Input);
            return destinationNode;
        }

        public void AddToInput(TInput input) => this.Input.Write(input);

        public Pipe<TInput> GetInput() => Input;

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
                    TInput item = Input.Read();
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