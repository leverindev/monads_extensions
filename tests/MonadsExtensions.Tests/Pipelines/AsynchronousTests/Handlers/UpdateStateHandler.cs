using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Database.Entities;
using MonadsExtensions.Tests.Pipelines.Models;

namespace MonadsExtensions.Tests.Pipelines.AsynchronousTests.Handlers;

public class UpdateStateHandler<T>(UpdateUserBalanceState state) : IPipeline<Result<T, string>, Result<T, string>>
{
    public Result<T, string> Execute(Result<T, string> input)
    {
        if (input.IsError(out var value, out var error))
        {
            return Result.Error(error);
        }

        switch (value)
        {
            case User user:
                state.User = user;
                break;
            case UserBalance userBalance:
                state.UserBalance = userBalance;
                break;
        }

        return input;
    }
}
