using System.Collections.Concurrent;

namespace DataflowPipelines
{
    public sealed class Pipe<T>
    {
        public BlockingCollection<T> PipeStorage { get; set; }
        private ConcurrentBag<Pipe<T>> pipeStreams;
        public bool HasStreamers { get => pipeStreams.Count > 0; }

        public Pipe()
        {
            PipeStorage = new BlockingCollection<T>();  
            pipeStreams = new ConcurrentBag<Pipe<T>>();
        }

        /// <summary>
        /// Writes item into pipe or streams item into different node if any
        /// </summary>
        /// <param name="item"></param>
        public void Write(T item)
        {
            if (pipeStreams.IsEmpty)
                this.PipeStorage.Add(item);

            foreach (var pipe in pipeStreams)
            {
                pipe.Write(item);
            }
        }

        /// <summary>
        /// Reads item from pipe
        /// </summary>
        /// <remarks>This is blocking call</remarks>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public T Read(CancellationTokenSource cancellationToken)
        {
            return this.PipeStorage.Take(cancellationToken.Token);
        }

        /// <summary>
        /// Reads item from pipe
        /// </summary>
        /// <remarks>This is blocking call</remarks>
        /// <returns></returns>
        public T Read()
        {
            return this.PipeStorage.Take();
        }

        /// <summary>
        /// Reads item from pipe
        /// </summary>
        /// <remarks>Non blocking call</remarks>
        /// <param name="item"></param>
        /// <returns><c>true</c> if any item read</returns>
        public bool TryRead(out T item)
        {
            return this.PipeStorage.TryTake(out item);
        }

        /// <summary>
        /// Duplicates pipe output to destination pipe
        /// </summary>
        /// <param name="destinationPipe"></param>
        /// <remarks>This method can be chained</remarks>
        /// <returns>Destinatination pipe</returns>
        public Pipe<T> StreamTo(Pipe<T> destinationPipe)
        {
            this.pipeStreams.Add(destinationPipe);
            return destinationPipe;
        }

        public IEnumerable<T> ReadAll()
        {
            List<T> items = new List<T>();
            while (this.PipeStorage.TryTake(out T item))
            {
                items.Add(item);
            }
            return items;
        }

    }
}
