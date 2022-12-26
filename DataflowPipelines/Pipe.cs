using System.Collections.Concurrent;

namespace DataflowPipelines
{
    public class Pipe<T>
    {
        public BlockingCollection<T> PipeStorage { get; set; }

        public Pipe()
        {
            PipeStorage = new BlockingCollection<T>();  
        }

        public void Write(T item)
        {
            this.PipeStorage.Add(item);
        }

        public T Read()
        {
            return this.PipeStorage.Take();
        }

    }
}
