using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines
{
    internal class AsyncSequentialPipeline<TInput, TOutput, T> : IAsyncPipeline<TInput, TOutput>
    {
        private readonly IAsyncPipeline<TInput, T> _inputPipeline;
        private readonly IAsyncPipeline<T, TOutput> _outputPipeline;

        public AsyncSequentialPipeline(IAsyncPipeline<TInput, T> inputPipeline, IAsyncPipeline<T, TOutput> outputPipeline)
        {
            _inputPipeline = inputPipeline ?? throw new ArgumentNullException(nameof(inputPipeline));
            _outputPipeline = outputPipeline ?? throw new ArgumentNullException(nameof(outputPipeline));
        }

        public async Task<TOutput> ExecuteAsync(TInput input)
        {
            return await _outputPipeline.ExecuteAsync(await _inputPipeline.ExecuteAsync(input));
        }
    }
}
