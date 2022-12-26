namespace DataflowPipelines;

public interface INode 
{
    
        // /// <summary>
        // /// Connects this node to destination node:
        // /// (this_node)-[:OUTPUT]-[:INPUT]->(destination_node)
        // /// </summary>
        // /// <param name="destinationNode"></param>
        // /// <returns>Destination node</returns>
        // INode ConnectTo(INode destinationNode);
        //
        // // void AddToInput(TInput input);
        // // Pipe<TOutput> GetOutput();
        
        void StartProcessing();
        //void AddToInput<T, TInput, TOutput>(TInput input) where T : Node<TInput, TOutput>;
        

}