using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Tests.ResultContainer;

public abstract class ResultTestsBase
{
    protected Result<int, string> ParseInt32(string input)
    {
        if (int.TryParse(input, out var value))
        {
            return Result.Ok(value);
        }

        return Result.Error("Invalid input");
    }

    protected Result<int, string> ParseInt32V2(string input)
    {
        if (int.TryParse(input, out var value))
        {
            return Result<int, string>.CreateOk(value);
        }

        return Result<int, string>.CreateError("Invalid input");
    }
}
