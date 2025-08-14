using MonadsExtensions.Pipelines.Async;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Models;

namespace MonadsExtensions.Tests.Pipelines.AsynchronousTests.Handlers;

public class ModifyBalanceAsyncHandler : IAsyncPipeline<Result<UpdateUserBalanceState, string>, Result<decimal, string>>
{
    public const string InsufficientFundsError = "Insufficient funds";

    public async Task<Result<decimal, string>> ExecuteAsync(Result<UpdateUserBalanceState, string> input)
    {
        if (input.IsError(out var state, out var error))
        {
            return Result.Error(error);
        }

        if (state.UserBalance is null)
        {
            throw new ArgumentNullException(nameof(state.UserBalance), "Unexpected user balance is null");
        }

        var newBalance = state.UserBalance.Balance + state.Transaction.Amount;
        if (newBalance < 0)
        {
            return Result.Error(InsufficientFundsError);
        }

        await Task.Yield(); // Simulate async work

        state.UserBalance.Balance = newBalance;

        return Result.Ok(state.UserBalance.Balance);
    }
}
