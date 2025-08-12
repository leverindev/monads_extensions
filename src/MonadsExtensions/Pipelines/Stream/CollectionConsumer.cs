using System.Collections.Generic;

namespace MonadsExtensions.Pipelines.Stream
{
    public class CollectionConsumer<T> : IConsumer<T>
    {
        private readonly List<T> _collection = new List<T>();

        public void Push(T item)
        {
            _collection.Add(item);
        }

        public IEnumerable<T> GetResult()
        {
            return _collection;
        }
    }
}
