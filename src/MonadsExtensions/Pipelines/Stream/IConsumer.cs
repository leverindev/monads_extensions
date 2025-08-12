namespace MonadsExtensions.Pipelines.Stream
{
    public interface IConsumer<in T>
    {
        void Push(T item);
    }
}
