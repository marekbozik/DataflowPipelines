using System.Collections.Concurrent;

namespace DataflowPipelines
{
    public class Pipe<T>
    {
        public BlockingCollection<T> PipeStorage { get; set; }
        private ConcurrentBag<Pipe<T>> pipeStreams;

        public Pipe()
        {
            PipeStorage = new BlockingCollection<T>();  
            pipeStreams = new ConcurrentBag<Pipe<T>>();
        }

        public void Write(T item)
        {
            this.PipeStorage.Add(item);
            foreach (var pipe in pipeStreams)
            {
                pipe.Write(item);
            }
        }

        public T Read()
        {
            return this.PipeStorage.Take();
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
