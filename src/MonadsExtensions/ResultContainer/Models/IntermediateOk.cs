namespace MonadsExtensions.ResultContainer.Models;

public readonly struct IntermediateOk<T>(T value)
{
    public T Value { get; } = value;
}
