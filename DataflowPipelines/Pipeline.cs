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
        //public LinkedList<INode> RootNodes { get; }


        public Pipeline()
        {
            Nodes = new LinkedList<INode>();
            //RootNodes = new LinkedList<INode>();
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

        //public void AddRootNode<T>(Node<TInput,T> node)
        //{
        //    RootNodes.AddLast(node);
        //}

        public void Compose()
        {
            foreach (var node in Nodes)
            {
               node.StartProcessing(); 
            }

            //foreach (var rootNode in RootNodes)
            //{
            //    rootNode.StartProcessing();
            //}
        }

    }
}
