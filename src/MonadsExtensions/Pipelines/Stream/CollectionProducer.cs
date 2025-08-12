using System.Collections.Concurrent;

namespace MonadsExtensions.Pipelines.Stream
{
    public class CollectionProducer<T> : IProducer<T>
    {
        private readonly BlockingCollection<T> _collection = new BlockingCollection<T>();

        public void Push(T item)
        {
            _collection.Add(item);
        }

        public void Complete()
        {
            _collection.CompleteAdding();
        }

        public T Pop()
        {
            return _collection.Take();
        }

        public bool IsCompleted()
        {
            return _collection.IsCompleted;
        }
    }
}
