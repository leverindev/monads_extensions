using System;

namespace MonadsExtensions.Pipelines
{
    internal class MapPipeline<TInput, TOutput, T> : IPipeline<TInput, TOutput>
    {
        private readonly IPipeline<TInput, T> _inputPipeline;
        private readonly Func<T, TOutput> _mapFunc;

        public MapPipeline(IPipeline<TInput, T> inputPipeline, Func<T, TOutput> mapFunc)
        {
            _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
            _mapFunc = mapFunc ?? throw new ArgumentNullException(nameof(mapFunc));
        }

        public TOutput Execute(TInput input)
        {
            return _mapFunc(_inputPipeline.Execute(input));
        }
    }
}
