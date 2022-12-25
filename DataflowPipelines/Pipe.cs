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

    }
}
