using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;
using MonadsExtensions.Tests.Pipelines.Models;
using MonadsExtensions.Tests.Pipelines.SynchronousTests.Handlers;

namespace MonadsExtensions.Tests.Pipelines.SynchronousTests.Services;

public class UpdateUserBalanceService(DatabaseContext database)
{
    public Result<decimal, string> UpdateUserBalance(UserBalanceTransaction transaction)
    {
        var state = new UpdateUserBalanceState { Transaction = transaction };

        var pipeline = new GetUserHandler(database)
            .PipeTo(new UpdateStateHandler<User>(state))
            .PipeTo(new GetUserBalanceHandler(database))
            .PipeTo(new UpdateStateHandler<UserBalance>(state))
            .PipeTo(x => x.Bind(b => state))
            .PipeTo(new ModifyBalanceHandler());

        return pipeline.Execute(transaction.UserId);
    }
}
