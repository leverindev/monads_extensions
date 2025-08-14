using MonadsExtensions.Pipelines.Async;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.AsynchronousTests.Handlers;

public class GetUserBalanceAsyncHandler(DatabaseContext database) : IAsyncPipeline<Result<User, string>, Result<UserBalance, string>>
{
    public async Task<Result<UserBalance, string>> ExecuteAsync(Result<User, string> input)
    {
        if (input.IsError(out var user, out var error))
        {
            return Result.Error(error);
        }

        await Task.Yield(); // Simulate async work

        var userBalance = database.UserBalances.FirstOrDefault(x => x.Id == user.Id);
        if (userBalance != null)
        {
            return Result.Ok(userBalance);
        }

        return Result.Error($"User balance with id {user.Id} not found");
    }
}
