using System;

namespace MonadsExtensions.Pipelines.Sync;

internal class MapPipeline<TInput, TOutput, T>(IPipeline<TInput, T> inputPipeline, Func<T, TOutput> mapFunc) : IPipeline<TInput, TOutput>
{
    private readonly IPipeline<TInput, T> _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
    private readonly Func<T, TOutput> _mapFunc = mapFunc ?? throw new ArgumentNullException(nameof(mapFunc));

    public TOutput Execute(TInput input)
    {
        return _mapFunc(_inputPipeline.Execute(input));
    }
}
