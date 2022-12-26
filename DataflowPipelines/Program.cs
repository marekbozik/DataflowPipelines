using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowPipelines
{
    public class Program
    {

        public static void Main()
        {
            // new Pipeline<string>();
            // var n1= Node<string, int>.CreateNode(word =>
            // {
            //     return 0;
            // });
            //
            // var n2 = Node<int, int>.CreateNode(word =>
            // {
            //     return 0;
            // });
            //
            // var n3 =  Node<int, string>.CreateNode(word =>
            // {
            //     return String.Empty;
            // });
            //
            // n1.ConnectTo(n2).ConnectTo(n3).ConnectTo(n1);



            var additionNode = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);

            var divideNode = Node<int, int>.CreateNode(sum => sum / 2);

            additionNode.ConnectTo(divideNode);
            
            var middleNumberPipeline = new Pipeline();
            middleNumberPipeline.AddNodes(additionNode, divideNode);

            var sink = Sink<int>.CreateSink(divideNode);
            var source = Source<(int, int)>.CreateSource(additionNode);
            
            middleNumberPipeline.Compose();


            source.Add((10, 0));
            Console.WriteLine(sink.Get());
        }
    }
}
