namespace MonadsExtensions.Pipelines
{
    public interface IPipeline<in TInput, out TOutput>
    {
        TOutput Execute(TInput input);
    }
}
