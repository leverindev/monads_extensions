using System;
using MonadsExtensions.Pipelines.Sync;

namespace MonadsExtensions.Pipelines.Async;

public static class PipeExtensions
{
    public static IAsyncPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(
        this IAsyncPipeline<TInput, T> inputPipeline,
        IAsyncPipeline<T, TOutput> outputPipeline)
    {
        return new AsyncSequentialPipeline<TInput, TOutput, T>(inputPipeline, outputPipeline);
    }

    public static IAsyncPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(
        this IAsyncPipeline<TInput, T> inputPipeline,
        Func<T, TOutput> mapFunc)
    {
        return new AsyncMapPipeline<TInput, TOutput, T>(inputPipeline, mapFunc);
    }

    public static IAsyncPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(
        this IPipeline<TInput, T> inputPipeline,
        IAsyncPipeline<T, TOutput> outputPipeline)
    {
        return new AsyncSequentialPipeline<TInput, TOutput, T>(new AsyncPipelineWrapper<TInput, T>(inputPipeline), outputPipeline);
    }

    public static IAsyncPipeline<TInput, TOutput> PipeTo<TInput, TOutput, T>(
        this IAsyncPipeline<TInput, T> inputPipeline,
        IPipeline<T, TOutput> outputPipeline)
    {
        return new AsyncSequentialPipeline<TInput, TOutput, T>(inputPipeline, new AsyncPipelineWrapper<T, TOutput>(outputPipeline));
    }
}
