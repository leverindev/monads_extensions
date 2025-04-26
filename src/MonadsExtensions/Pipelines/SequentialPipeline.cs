using System;

namespace MonadsExtensions.Pipelines
{
    internal class SequentialPipeline<TInput, TOutput, T> : IPipeline<TInput, TOutput>
    {
        private readonly IPipeline<TInput, T> _inputPipeline;
        private readonly IPipeline<T, TOutput> _outputPipeline;

        public SequentialPipeline(IPipeline<TInput, T> inputPipeline, IPipeline<T, TOutput> outputPipeline)
        {
            _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
            _outputPipeline = outputPipeline ?? throw new ArgumentNullException(nameof(outputPipeline));
        }

        public TOutput Execute(TInput input)
        {
            return _outputPipeline.Execute(_inputPipeline.Execute(input));
        }
    }
}
