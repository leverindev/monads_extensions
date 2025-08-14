using MonadsExtensions.Pipelines.Async;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.AsynchronousTests.Handlers;

public class GetUserAsyncHandler(DatabaseContext database) : IAsyncPipeline<long, Result<User, string>>
{
    public const string UserNotFoundError = "User not found";

    public async Task<Result<User, string>> ExecuteAsync(long userId)
    {
        await Task.Yield(); // Simulate async work

        var user = database.Users.FirstOrDefault(x => x.Id == userId);
        if (user != null)
        {
            return Result.Ok(user);
        }

        return Result.Error(UserNotFoundError);
    }
}
