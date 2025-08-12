using MonadsExtensions.Pipelines.Sync;

namespace MonadsExtensions.Pipelines.Stream;

public interface IStreamPipeline<TInput, TOutput>
{
    void SetProducer(IProducer<TInput> producer);

    void SetConsumer(IConsumer<TOutput> consumer);

    void SetPipeline(IPipeline<TInput, TOutput> pipeline);

    void Run();
}
