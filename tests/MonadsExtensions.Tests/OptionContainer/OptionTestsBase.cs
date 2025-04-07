using MonadsExtensions.OptionContainer;

namespace MonadsExtensions.Tests.OptionContainer;

public abstract class OptionTestsBase
{
    protected Option<bool> IsEven(string input)
    {
        if (int.TryParse(input, out var value))
        {
            return Option.Some(value % 2 == 0);
        }

        return Option.None;
    }

    protected Option<bool> IsEvenV2(string input)
    {
        if (int.TryParse(input, out var value))
        {
            return Option<bool>.Some(value % 2 == 0);
        }

        return Option<bool>.None;
    }
}
