using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines
{
    internal class AsyncMapPipeline<TInput, TOutput, T> : IAsyncPipeline<TInput, TOutput>
    {
        private readonly IAsyncPipeline<TInput, T> _inputPipeline;
        private readonly Func<T, TOutput> _mapFunc;

        public AsyncMapPipeline(IAsyncPipeline<TInput, T> inputPipeline, Func<T, TOutput> mapFunc)
        {
            _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
            _mapFunc = mapFunc ?? throw new ArgumentNullException(nameof(mapFunc));
        }

        public async Task<TOutput> ExecuteAsync(TInput input)
        {
            return _mapFunc(await _inputPipeline.ExecuteAsync(input));
        }
    }
}
