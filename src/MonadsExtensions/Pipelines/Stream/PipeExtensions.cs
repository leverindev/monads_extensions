using MonadsExtensions.Pipelines.Sync;

namespace MonadsExtensions.Pipelines.Stream;

public static class PipeExtensions
{
    public static IStreamPipeline<TInput, TOutput> Create<TInput, TOutput>()
    {
        return new GenericStreamPipeline<TInput, TOutput>();
    }

    public static IStreamPipeline<TInput, TOutput> AddProducer<TInput, TOutput>(
        this IStreamPipeline<TInput, TOutput> streamPipeline,
        IProducer<TInput> producer)
    {
        streamPipeline.SetProducer(producer);

        return streamPipeline;
    }

    public static IStreamPipeline<TInput, TOutput> AddConsumer<TInput, TOutput>(
        this IStreamPipeline<TInput, TOutput> streamPipeline,
        IConsumer<TOutput> consumer)
    {
        streamPipeline.SetConsumer(consumer);

        return streamPipeline;
    }

    public static IStreamPipeline<TInput, TOutput> AddPipe<TInput, TOutput>(
        this IStreamPipeline<TInput, TOutput> streamPipeline,
        IPipeline<TInput, TOutput> pipeline)
    {
        streamPipeline.SetPipeline(pipeline);

        return streamPipeline;
    }
}
