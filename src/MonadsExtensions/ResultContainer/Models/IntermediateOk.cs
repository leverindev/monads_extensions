namespace MonadsExtensions.ResultContainer.Models
{
    public readonly struct IntermediateOk<T>
    {
        public T Value { get; }

        public IntermediateOk(T value)
        {
            Value = value;
        }
    }
}
