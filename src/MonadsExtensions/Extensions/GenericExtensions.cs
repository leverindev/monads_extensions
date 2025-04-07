using System;

namespace MonadsExtensions.Extensions
{
    public static class GenericExtensions
    {
        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
        {
            return func is null ? default : func(source);
        }

        public static T Do<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }

            return obj;
        }
    }
}
