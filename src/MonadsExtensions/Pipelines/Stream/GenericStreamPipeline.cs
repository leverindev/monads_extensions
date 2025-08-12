using MonadsExtensions.Pipelines.Sync;
using System;

namespace MonadsExtensions.Pipelines.Stream
{
    public class GenericStreamPipeline<TInput, TOutput> : IStreamPipeline<TInput, TOutput>
    {
        private IProducer<TInput> _producer;
        private IConsumer<TOutput> _consumer;
        private IPipeline<TInput, TOutput> _pipeline;

        public void SetProducer(IProducer<TInput> producer)
        {
            _producer = producer;
        }

        public void SetConsumer(IConsumer<TOutput> consumer)
        {
            _consumer = consumer;
        }

        public void SetPipeline(IPipeline<TInput, TOutput> pipeline)
        {
            _pipeline = pipeline;
        }

        public void Run()
        {
            if (_producer is null)
            {
                throw new InvalidOperationException("Producer is not set.");
            }

            if (_consumer is null)
            {
                throw new InvalidOperationException("Consumer is not set.");
            }

            if (_pipeline is null)
            {
                throw new InvalidOperationException("Pipe is not set.");
            }

            while (!_producer.IsCompleted())
            {
                var item = _producer.Pop();
                var result = _pipeline.Execute(item);
                _consumer.Push(result);
            }
        }
    }
}
