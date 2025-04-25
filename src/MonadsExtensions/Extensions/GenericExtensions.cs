using System;
using System.Threading.Tasks;

namespace MonadsExtensions.Extensions
{
    public static class GenericExtensions
    {
        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
        {
            return func is null ? default : func(source);
        }

        public static Task<TResult> MapAsync<TSource, TResult>(this Task<TSource> sourceTask, Func<Task<TSource>, Task<TResult>> func)
        {
            return sourceTask == null ? default : func?.Invoke(sourceTask);
        }

        public static Task<TResult> MapAsync<TSource, TResult>(this Task<TSource> sourceTask, Func<TSource, TResult> func)
        {
            return sourceTask?.ContinueWith(task => func(task.Result));
        }

        public static Task<TResult> MapAsync<TSource, TResult>(this Task<TSource> sourceTask, Func<Task<TSource>, TResult> func)
        {
            return sourceTask?.ContinueWith(func);
        }

        public static T Do<T>(this T obj, Action<T> action)
        {
            if (obj == null)
            {
                return default;
            }

            if (action == null)
            {
                return obj;
            }

            action(obj);

            return obj;
        }

        public static Task<T> DoAsync<T>(this Task<T> task, Action<Task<T>> action)
        {
            if (task == null)
            {
                return default;
            }

            if (action == null)
            {
                return task;
            }

            action(task);

            return task;
        }

        public static async Task<T> DoAsync<T>(this Task<T> task, Action<T> action)
        {
            if (task == null)
            {
                return default;
            }

            var obj = await task;

            return obj.Do(action);
        }

        public static async Task<T> DoAsync<T>(this Task<T> task, Func<Task<T>, Task> action)
        {
            if (task == null)
            {
                return default;
            }

            if (action == null)
            {
                return await task;
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

            var obj = await task;

            await action(obj);

            return obj;
        }
    }
}
