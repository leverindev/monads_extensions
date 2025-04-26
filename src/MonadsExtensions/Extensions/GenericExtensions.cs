using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Extensions
{
    public static class GenericExtensions
    {
        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return func(source);
        }

        public static Task<TResult> MapAsync<TSource, TResult>(this Task<TSource> sourceTask, Func<TSource, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return sourceTask?.ContinueWith(task => task.Result.Map(func));
        }

        public static Task<TResult> MapAsync<TSource, TResult>(this Task<TSource> sourceTask, Func<Task<TSource>, TResult> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return sourceTask?.ContinueWith(func);
        }

        public static T Do<T>(this T obj, Action<T> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (obj == null)
            {
                return default;
            }

            action(obj);

            return obj;
        }

        public static Task<T> DoAsync<T>(this Task<T> task, Action<T> action)
        {
            return task?.ContinueWith(t => t.Result.Do(action));
        }

        public static async Task<T> DoAsync<T>(this Task<T> task, Func<Task<T>, Task> action)
        {
            if (task == null)
            {
                return default;
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            await action(task);

            return await task;
        }

        public static async Task<T> DoAsync<T>(this Task<T> task, Func<T, Task> action)
        {
            if (task == null)
            {
                return default;
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var obj = await task;

            await action(obj);

            return obj;
        }
    }
}
