using System;

namespace MonadsExtensions.Pipelines.Sync;

internal class SequentialPipeline<TInput, TOutput, T>(IPipeline<TInput, T> inputPipeline, IPipeline<T, TOutput> outputPipeline)
    : IPipeline<TInput, TOutput>
{
    private readonly IPipeline<TInput, T> _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
    private readonly IPipeline<T, TOutput> _outputPipeline = outputPipeline ?? throw new ArgumentNullException(nameof(outputPipeline));

    public TOutput Execute(TInput input)
    {
        return _outputPipeline.Execute(_inputPipeline.Execute(input));
    }
}
