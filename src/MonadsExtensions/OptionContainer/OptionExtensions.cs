using System;
using System.Threading.Tasks;
using MonadsExtensions.OptionContainer.Models;

namespace MonadsExtensions.OptionContainer;

public static class Option
{
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    public static None None { get; } = new ();

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

    public static Option<TResult> Bind<TInput, TResult>(this Option<TInput> result, Func<TInput, TResult> map)
    {
        return result.HasValue ? Some(map(result.Value)) : None;
    }

    public static Task<Option<TResult>> BindAsync<TInput, TResult>(this Task<Option<TInput>> resultTask, Func<TInput, TResult> map)
    {
        return resultTask?.ContinueWith(task => Bind(task.Result, map));
    }
}
