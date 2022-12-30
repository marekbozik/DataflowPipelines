using System.Collections.Concurrent;

namespace DataflowPipelines
{

    public abstract class Node<TInput, TOutput> : INode
    {

        public Pipe<TInput> Input { get; set; } 
        private ConcurrentBag<Pipe<TOutput>> outputs;
        private Pipe<TOutput> output;
        private TOutput lastOutput;
        private Task processingTask;
        private CancellationTokenSource cancellationToken;
        protected Node()
        {
            Input = new Pipe<TInput>();
            outputs = new ConcurrentBag<Pipe<TOutput>>();
            output = new Pipe<TOutput>();
            cancellationToken = new CancellationTokenSource();
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
            this.output.StreamTo(destinationNode.Input);
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

        public async Task CancelProcessing()
        {
            cancellationToken.Cancel();
            try
            {
                await processingTask;
            }
            catch (OperationCanceledException)
            {
                ;
            }
            finally
            {
                cancellationToken.Dispose();
            }
            cancellationToken = new CancellationTokenSource();
        }

        public void StartProcessing()
        {
            processingTask = new Task(() =>
            {
                while (true)
                {
                    TInput item;
                    try
                    {
                        item = Input.Read(this.cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                       break;
                    }

                    TOutput outputItem;

                    outputItem = this.Run(item);

                    if (this.cancellationToken.IsCancellationRequested)
                        break;

                    if (output.HasStreamers)
                        output.Write(outputItem);

                    foreach (var pipe in outputs)
                    {
                        pipe.Write(outputItem);
                    }
                    lastOutput = outputItem;

                }
            }, this.cancellationToken.Token, TaskCreationOptions.LongRunning);

            processingTask.Start();
        }
    }
}