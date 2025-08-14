using MonadsExtensions.Pipelines.Async;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.AsynchronousTests.Handlers;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;
using MonadsExtensions.Tests.Pipelines.Models;

namespace MonadsExtensions.Tests.Pipelines.AsynchronousTests.Services;

public class UpdateUserBalanceAsyncService(DatabaseContext database)
{
    public async Task<Result<decimal, string>> UpdateUserBalanceAsync(UserBalanceTransaction transaction)
    {
        var state = new UpdateUserBalanceState { Transaction = transaction };

        var pipeline = new GetUserAsyncHandler(database)
            .PipeTo(new UpdateStateHandler<User>(state))
            .PipeTo(new GetUserBalanceAsyncHandler(database))
            .PipeTo(new UpdateStateHandler<UserBalance>(state))
            .PipeTo(x => x.Bind(_ => state))
            .PipeTo(new ModifyBalanceAsyncHandler());

        return await pipeline.ExecuteAsync(transaction.UserId);
    }
}
