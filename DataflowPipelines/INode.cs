namespace DataflowPipelines;

public interface INode 
{
    void StartProcessing();
    Task CancelProcessing();
}