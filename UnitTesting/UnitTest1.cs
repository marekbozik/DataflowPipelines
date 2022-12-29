using DataflowPipelines;

namespace UnitTesting
{
    public class Tests
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
            //Console.WriteLine(sink.Get()); //output: 5

            source.Add((50, 100));
            Assert.AreEqual(75, sink.Get());
            //Console.WriteLine(sink.Get()); //output: 75
        }

        [Test]
        public void Test2()
        {
            var additionNode = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);
            var additionNode2 = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);

            var divideNode = Node<int, int>.CreateNode(sum => sum / 2);

            additionNode.ConnectTo(divideNode);
            additionNode2.ConnectTo(divideNode);

            var middleNumberPipeline = new Pipeline();
            middleNumberPipeline.AddNodes(additionNode, additionNode2, divideNode);

            var source = Source<(int, int)>.CreateSource(additionNode);
            var source2 = Source<(int, int)>.CreateSource(additionNode2);

            var sink = Sink<int>.CreateSink(divideNode);

            middleNumberPipeline.Compose();

            source.Add((10, 0));
            Assert.AreEqual(5, sink.Get());

            source2.Add((10, 0));
            Assert.AreEqual(5, sink.Get());

            source.Add((50, 100));
            Assert.AreEqual(75, sink.Get());
        }

        [Test]
        public void Test3()
        {
            var additionNode = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);
            var additionNode2 = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);

            var divideNode = Node<int, int>.CreateNode(sum => sum / 2);

            additionNode.ConnectTo(divideNode);
            additionNode2.ConnectTo(divideNode);

            var middleNumberPipeline = new Pipeline();
            middleNumberPipeline.AddNodes(additionNode, additionNode2, divideNode);

            var source = Source<(int, int)>.CreateSource(additionNode);
            var source2 = Source<(int, int)>.CreateSource(additionNode2);

            var sink = Sink<int>.CreateSink(divideNode);

            middleNumberPipeline.Compose();

            source.Add((10, 0));
            source2.Add((10, 20));
            source.Add((50, 100));

            List<int> ints = new List<int>();
            for (int i = 0; i < 3; i++)
                ints.Add(sink.Get());


            Assert.IsTrue(ints.Contains(5));
            Assert.IsTrue(ints.Contains(15));
            Assert.IsTrue(ints.Contains(75));
        }

        [Test]
        public void Test4()
        {
            var additionNode = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);
            var additionNode2 = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);

            var divideNode = Node<int, int>.CreateNode(sum => sum / 2);

            additionNode.ConnectTo(divideNode);
            additionNode2.ConnectTo(divideNode);

            var middleNumberPipeline = new Pipeline();
            middleNumberPipeline.AddNodes(additionNode, additionNode2, divideNode);

            var source = Source<(int, int)>.CreateSource(additionNode);
            var source2 = Source<(int, int)>.CreateSource(additionNode2);

            var sink = Sink<int>.CreateSink(divideNode);
            var sink2 = Sink<int>.CreateSink(divideNode);

            middleNumberPipeline.Compose();

            source.Add((10, 0));
            Assert.AreEqual(5, sink.Get());

            source2.Add((10, 20));
            Assert.AreEqual(15, sink.Get());

            source.Add((50, 100));
            Assert.AreEqual(75, sink.Get());

            List<int> ints = new List<int>();
            for (int i = 0; i < 3; i++)
                ints.Add(sink2.Get());


            Assert.IsTrue(ints.Contains(5));
            Assert.IsTrue(ints.Contains(15));
            Assert.IsTrue(ints.Contains(75));
        }
    }
}