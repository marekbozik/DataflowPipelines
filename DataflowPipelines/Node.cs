using System.Collections.Concurrent;

namespace DataflowPipelines
{

    public abstract class Node<TInput, TOutput> : INode
    {
        private Func<TInput, TOutput>? runFunction;
        public Pipe<TInput> Input { get; set; } 
        private ConcurrentBag<Pipe<TInput>> destinationInputs;
        private ConcurrentBag<Pipe<TOutput>> outputs;
        private TOutput lastOutput;

        protected Node()
        {
            Input = new Pipe<TInput>();
            destinationInputs = new ConcurrentBag<Pipe<TInput>>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        protected Node(Func<TInput, TOutput> runFunction)
        {
            this.runFunction = runFunction;
            Input = new Pipe<TInput>();
            destinationInputs = new ConcurrentBag<Pipe<TInput>>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        public static Node<TInput, TOutput> CreateNode(Func<TInput, TOutput> runFunction)
        {
            return new SimpleNode<TInput, TOutput>(runFunction);
        }

        public abstract TOutput Run(TInput input);
        
        private void AddDestinationInput()
        {
            this.destinationInputs.Add(Input);
        }

        /// <summary>
        /// Connects this node to destination node:
        /// (this_node)-[:OUTPUT]-[:INPUT]->(destination_node)
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <returns>Destination node</returns>
        public Node<TOutput, T> ConnectTo<T>(Node<TOutput, T> destinationNode)
        {
            //destinationNode.input = this.GetOutput();
            this.GetOutput().StreamTo(destinationNode.Input);
            //this.destinationInputs.Add(destinationNode.Input);
            //destinationNode.destinationInputs.Add(this.GetOutput());
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
                    //foreach(var i in inputs)
                    //{
                    //    foreach (var items in i.ReadAll())
                    //    {
                    //        input.Write(items);
                    //    }
                    //}
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