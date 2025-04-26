using System;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Models;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.PipelineExample.Handlers;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.PipelineExample
{
    public class Service
    {
        public static readonly DatabaseContext Database = new DatabaseContext();

        public void UpdateUserBalance(UserBalanceTransaction transaction)
        {
            var state = new UpdateUserBalanceState { Transaction = transaction };

            var pipeline = new GetUserHandler()
                .PipeTo(new UpdateStateHandler<User>(state))
                .PipeTo(new GetUserBalanceHandler())
                .PipeTo(new UpdateStateHandler<UserBalance>(state))
                .PipeTo(x => x.Bind(b => state))
                .PipeTo(new ModifyBalanceHandler());

            var result = pipeline.Execute(transaction.UserId);

            Console.WriteLine(
                result.IsValue(out var newBalance, out var error)
                    ? $"New balance: {newBalance}"
                    : $"Transaction error: {error}");
        }
    }
}
