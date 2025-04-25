namespace MonadsExtensions.Pipelines
{
    public interface IConsumer<in T>
    {
        void Push(T item);
    }
}
