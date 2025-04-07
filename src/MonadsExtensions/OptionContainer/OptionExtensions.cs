using System;
using MonadsExtensions.OptionContainer.Models;

namespace MonadsExtensions.OptionContainer
{
    public static class Option
    {
        public static Option<T> Some<T>(T value) => Option<T>.Some(value);

        public static None None { get; } = new None();

        public static Option<T> ToOption<T>(this T value) => Some(value);

        public static T UnwrapOrDefault<T>(this Option<T> result)
        {
            return result.HasValue ? result.Value : default;
        }

        public static T UnwrapOrException<T>(this Option<T> result, Exception exception)
        {
            if (result.HasValue)
            {
                return result.Value;
            }

            throw exception;
        }

        public static T UnwrapOrException<T>(this Option<T> result, string message) =>
            result.UnwrapOrException(new Exception(message));

        public static TResult UnwrapOrException<TResult>(this Option<TResult> result) =>
            result.UnwrapOrException(new ArgumentNullException(nameof(result)));
    }
}
