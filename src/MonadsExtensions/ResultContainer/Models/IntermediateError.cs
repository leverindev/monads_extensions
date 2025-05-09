namespace MonadsExtensions.ResultContainer.Models;

public readonly struct IntermediateError<T>(T value)
{
    public T Value { get; } = value;
}
