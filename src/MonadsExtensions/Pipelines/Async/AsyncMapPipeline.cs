using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines.Async;

internal class AsyncMapPipeline<TInput, TOutput, T>(IAsyncPipeline<TInput, T> inputPipeline, Func<T, TOutput> mapFunc) : IAsyncPipeline<TInput, TOutput>
{
    private readonly IAsyncPipeline<TInput, T> _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
    private readonly Func<T, TOutput> _mapFunc = mapFunc ?? throw new ArgumentNullException(nameof(mapFunc));

    public async Task<TOutput> ExecuteAsync(TInput input)
    {
        return _mapFunc(await _inputPipeline.ExecuteAsync(input));
    }
}
