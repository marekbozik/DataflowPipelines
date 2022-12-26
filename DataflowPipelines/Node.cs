using System.Collections.Concurrent;

namespace DataflowPipelines
{

    public abstract class Node<TInput, TOutput> : INode
    {
        private Func<TInput, TOutput>? runFunction;


        private Pipe<TInput> inputs;
        private ConcurrentBag<Pipe<TOutput>> outputs;
        private TOutput lastOutput;

        //private BlockingCollection<TInput> inputs;
        //private BlockingCollection<TOutput> outputs;

        public Node()
        {
            inputs = new Pipe<TInput>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
        }

        public Node(Func<TInput, TOutput> runFunction)
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

        private void SetInput(Node<TOutput, TOutput> destinationNode)
        {
            destinationNode.inputs = this.GetOutput();
            //destinationNode.inputs = (Pipe<TInput>)Convert.ChangeType(this.GetOutput(), typeof(Pipe<TInput>));
        }
        

        // public Node<TOutput, TOutput> ConnectTo(Node<TOutput, TOutput> destinationNode)
        // {
        //     SetInput(destinationNode);
        //     return destinationNode;
        // }
        //
        // public Node<TOutput, TInput> ConnectTo(Node<TOutput, TInput> destinationNode)
        // {
        //     destinationNode.inputs = this.GetOutput();
        //     //SetInput(destinationNode);
        //     return destinationNode;
        // }
        
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

        //public void AddToInput(TInput input) => inputs.Write(input);

        public Pipe<TOutput> GetOutput()
        {
            var pipe = new Pipe<TOutput>();
            outputs.Add(pipe);
            return pipe;
        }

        // public INode ConnectTo(INode destinationNode)
        // {
        //     
        // }

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

        //public void AddToInput<T>(T input) where T : TInput
        //{
        //    inputs.Write(input);
        //}

        //public void AddToInput<T, TT>(T input) where T : Node<T, TT>
        //{
        //    inputs.Write(input);

        //    //throw new NotImplementedException();
        //}

        //public void AddToInput<T, TInput, TOutput>(TInput input) where T : Node<TInput, TOutput>
        //{
        //    inputs.Write(input);

        //}


        //public void AddToInput(TInput input)
        //{
        //    inputs.Write(input);
        //}
    }
}