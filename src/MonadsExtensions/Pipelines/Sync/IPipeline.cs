namespace MonadsExtensions.Pipelines.Sync;

public interface IPipeline<in TInput, out TOutput>
{
    TOutput Execute(TInput input);
}
