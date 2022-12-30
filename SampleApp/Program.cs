
using DataflowPipelines;

var additionNode = Node<(int, int), int>.CreateNode((nums) => nums.Item1 + nums.Item2);

var divideNode = Node<int, int>.CreateNode(sum => sum / 2);

additionNode.ConnectTo(divideNode);

var middleNumberPipeline = new Pipeline();
middleNumberPipeline.AddNodes(additionNode, divideNode);

var source = Source<(int, int)>.CreateSource(additionNode);
var sink = Sink<int>.CreateSink(divideNode);

middleNumberPipeline.Compose();

source.Add((10, 0));
Console.WriteLine(sink.Get()); //output: 5

source.Add((50, 100));
Console.WriteLine(sink.Get()); //output: 75

middleNumberPipeline.Down();
