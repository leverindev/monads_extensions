using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines.Async;

internal class AsyncSequentialPipeline<TInput, TOutput, T>(IAsyncPipeline<TInput, T> inputPipeline, IAsyncPipeline<T, TOutput> outputPipeline)
    : IAsyncPipeline<TInput, TOutput>
{
    private readonly IAsyncPipeline<TInput, T> _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
    private readonly IAsyncPipeline<T, TOutput> _outputPipeline = outputPipeline ?? throw new ArgumentNullException(nameof(outputPipeline));

    public async Task<TOutput> ExecuteAsync(TInput input)
    {
        return await _outputPipeline.ExecuteAsync(await _inputPipeline.ExecuteAsync(input));
    }
}
