using MonadsExtensions.OptionContainer.Models;

namespace MonadsExtensions.OptionContainer
{
    public static class Option
    {
        public static Option<T> Some<T>(T value) => Option<T>.Some(value);

        public static None None { get; } = new None();

        public static Option<T> ToOption<T>(this T value) => Some(value);
    }
}
