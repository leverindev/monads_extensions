using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.SynchronousTests.Handlers;

public class GetUserHandler(DatabaseContext database) : IPipeline<long, Result<User, string>>
{
    public const string UserNotFoundError = "User not found";

    public Result<User, string> Execute(long userId)
    {
        var user = database.Users.FirstOrDefault(x => x.Id == userId);
        if (user != null)
        {
            return Result.Ok(user);
        }

        return Result.Error(UserNotFoundError);
    }
}
