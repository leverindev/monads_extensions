namespace MonadsExtensions.OptionContainer
{
    public readonly struct Option<T>
    {
        private readonly T _value;

        private Option(T value, bool hasValue)
        {
            _value = value;
            HasValue = hasValue;
        }

        public Option(T value) : this(value, true)
        {
        }

        public T Value => HasValue ? _value : default;

        public bool HasValue { get; }

        public bool TryGetValue(out T value)
        {
            value = Value;

            return HasValue;
        }
    }
}
