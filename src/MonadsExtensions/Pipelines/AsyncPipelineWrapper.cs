using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Pipelines
{
    internal class AsyncPipelineWrapper<TInput, TOutput> : IAsyncPipeline<TInput, TOutput>
    {
        private readonly IPipeline<TInput, TOutput> _pipeline;

        public AsyncPipelineWrapper(IPipeline<TInput, TOutput> pipeline)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        public Task<TOutput> ExecuteAsync(TInput input)
        {
            return Task.FromResult(_pipeline.Execute(input));
        }
    }
}
