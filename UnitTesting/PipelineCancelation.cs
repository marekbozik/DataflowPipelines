using DataflowPipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class PipelineCancelation
    {
        [Test]
        public void Test1()
        {
            var additionNode = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);

            var divideNode = Node<int, int>.CreateNode(sum => sum / 2);

            additionNode.ConnectTo(divideNode);

            var middleNumberPipeline = new Pipeline();
            middleNumberPipeline.AddNodes(additionNode, divideNode);

            var source = Source<(int, int)>.CreateSource(additionNode);
            var sink = Sink<int>.CreateSink(divideNode);

            middleNumberPipeline.Compose();

            source.Add((10, 0));
            Assert.AreEqual(5, sink.Get());
            

            source.Add((50, 100));
            Assert.AreEqual(75, sink.Get());

            middleNumberPipeline.Down();

            var t1 = DateTime.Now;
            source.Add((10, 0));

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                middleNumberPipeline.Compose();
            });

            Assert.AreEqual(5, sink.Get());
            Assert.IsTrue((DateTime.Now - t1).TotalSeconds > 2);
        }
    }
}
