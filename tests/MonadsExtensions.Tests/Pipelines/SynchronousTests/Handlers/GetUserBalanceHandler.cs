using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.SynchronousTests.Handlers;

public class GetUserBalanceHandler(DatabaseContext database) : IPipeline<Result<User, string>, Result<UserBalance, string>>
{
    public Result<UserBalance, string> Execute(Result<User, string> input)
    {
        if (input.IsError(out var user, out var error))
        {
            return Result.Error(error);
        }

        var userBalance = database.UserBalances.FirstOrDefault(x => x.Id == user.Id);
        if (userBalance != null)
        {
            return Result.Ok(userBalance);
        }

        return Result.Error($"User balance with id {user.Id} not found");
    }
}
