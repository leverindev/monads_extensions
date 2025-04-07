namespace MonadsExtensions.ResultContainer.Models
{
    public readonly struct IntermediateError<T>
    {
        public T Value { get; }

        public IntermediateError(T value)
        {
            Value = value;
        }
    }
}
