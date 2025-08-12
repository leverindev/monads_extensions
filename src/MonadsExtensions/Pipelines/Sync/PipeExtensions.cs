using System;

namespace MonadsExtensions.Pipelines.Sync;

public static class PipeExtensions
{
    public static IPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(
        this IPipeline<TInput, T> inputPipeline,
        IPipeline<T, TOutput> outputPipeline)
    {
        return new SequentialPipeline<TInput, TOutput, T>(inputPipeline, outputPipeline);
    }

    public static IPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(
        this IPipeline<TInput, T> inputPipeline,
        Func<T, TOutput> mapFunc)
    {
        return new MapPipeline<TInput, TOutput, T>(inputPipeline, mapFunc);
    }
}
