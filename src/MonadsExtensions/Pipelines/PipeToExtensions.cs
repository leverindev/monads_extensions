namespace MonadsExtensions.Pipelines
{
    public static class PipeToExtensions
    {
        public static IStreamPipeline<TInput, TOutput> Create<TInput, TOutput>()
        {
            return new GenericStreamPipeline<TInput, TOutput>();
        }

        public static IStreamPipeline<TInput, TOutput> AddProducer<TInput, TOutput>(this IStreamPipeline<TInput, TOutput> streamPipeline, IProducer<TInput> producer)
        {
            streamPipeline.SetProducer(producer);

            return streamPipeline;
        }

        public static IStreamPipeline<TInput, TOutput> AddConsumer<TInput, TOutput>(this IStreamPipeline<TInput, TOutput> streamPipeline, IConsumer<TOutput> consumer)
        {
            streamPipeline.SetConsumer(consumer);

            return streamPipeline;
        }

        public static IStreamPipeline<TInput, TOutput> AddPipe<TInput, TOutput>(this IStreamPipeline<TInput, TOutput> streamPipeline, IPipeline<TInput, TOutput> pipeline)
        {
            streamPipeline.SetPipeline(pipeline);

            return streamPipeline;
        }

        public static IPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(this IPipeline<TInput, T> inputPipeline, IPipeline<T, TOutput> outputPipeline)
        {
            return new SequentialPipeline<TInput, TOutput, T>(inputPipeline, outputPipeline);
        }
    }
}
