using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    public class Pipeline
    {

        public LinkedList<INode> Nodes { get; }
        
        public Pipeline()
        {
            Nodes = new LinkedList<INode>();
        }

        public void AddNode(INode node)
        {
            Nodes.AddLast(node);
        }

        public void AddNodes(IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
            {
                Nodes.AddLast(node);
            }
        }

        public void AddNodes(params INode[] nodes)
        {
            foreach(var node in nodes)
            {
                Nodes.AddLast(node);
            }
        }

        /// <summary>
        /// Composes pipeline 
        /// </summary>
        public void Compose()
        {
            foreach (var node in Nodes)
            {
               node.StartProcessing(); 
            }
        }

        /// <summary>
        /// Stops pipeline processing
        /// </summary>
        public void Down()
        {
            List<Task> tasks = new(this.Nodes.Count);
            foreach (var node in this.Nodes)
            {
                tasks.Add(node.CancelProcessing());
            }
            Task.WaitAll(tasks.ToArray());
        }

    }
}
