namespace MonadsExtensions.Pipelines
{
    public interface IProducer<out T>
    {
        T Pop();

        bool IsCompleted();
    }
}
