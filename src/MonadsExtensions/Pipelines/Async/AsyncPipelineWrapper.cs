using MonadsExtensions.Pipelines.Sync;
using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines.Async;

internal class AsyncPipelineWrapper<TInput, TOutput>(IPipeline<TInput, TOutput> pipeline) : IAsyncPipeline<TInput, TOutput>
{
    private readonly IPipeline<TInput, TOutput> _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));

    public Task<TOutput> ExecuteAsync(TInput input)
    {
        return Task.FromResult(_pipeline.Execute(input));
    }
}
