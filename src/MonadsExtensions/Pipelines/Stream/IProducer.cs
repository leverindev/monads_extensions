namespace MonadsExtensions.Pipelines.Stream
{
    public interface IProducer<out T>
    {
        T Pop();

        bool IsCompleted();
    }
}
