using System;
using System.Threading.Tasks;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample.Handlers;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Models;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample
{
    public class AsyncService
    {
        public static readonly DatabaseContext Database = new DatabaseContext();

        public async Task UpdateUserBalanceAsync(UserBalanceTransaction transaction)
        {
            var state = new UpdateUserBalanceState { Transaction = transaction };

            var pipeline = new GetUserAsyncHandler()
                .PipeTo(new UpdateStateHandler<User>(state))
                .PipeTo(new GetUserBalanceAsyncHandler())
                .PipeTo(new UpdateStateHandler<UserBalance>(state))
                .PipeTo(x => x.Bind(b => state))
                .PipeTo(new ModifyBalanceAsyncHandler());

            var result = await pipeline.ExecuteAsync(transaction.UserId);

            Console.WriteLine(
                result.IsValue(out var newBalance, out var error)
                    ? $"New balance: {newBalance}"
                    : $"Transaction error: {error}");
        }
    }
}
