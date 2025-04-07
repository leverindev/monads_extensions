using System;
using MonadsExtensions.Extensions;

namespace MonadsExtensions.ResultContainer
{
    public readonly struct Result<TValue, TError>
    {
        public Result(TValue value, TError error, bool hasValue)
        {
            Value = value;
            Error = error;
            HasValue = hasValue;
        }

        public Result(TValue value) : this(value, default, true)
        {
        }

        public Result(TError error) : this(default, error, false)
        {
        }

        public TValue Value { get; }

        public TError Error { get; }

        public bool HasValue { get; }

        public void Do(Action<TValue> onSuccess, Action<TError> onError)
        {
            if (HasValue)
            {
                Value.Do(onSuccess);
            }
            else
            {
                Error.Do(onError);
            }
        }

        public T Bind<T>(Func<TValue, T> onSuccess, Func<TError, T> onError)
        {
            return HasValue ? Value.Map(onSuccess) : Error.Map(onError);
        }

        public void Deconstruct(out TValue value, out TError error)
        {
            value = Value;
            error = Error;
        }

        public bool IsValue(out TValue value, out TError error)
        {
            value = default;
            error = default;

            if (HasValue)
            {
                value = Value;
            }
            else
            {
                error = Error;
            }

            return HasValue;
        }

        public bool IsError(out TValue value, out TError error) => !IsValue(out value, out error);
    }
}
