using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.Pipelines.Models;

namespace MonadsExtensions.Tests.Pipelines.SynchronousTests.Handlers;

public class ModifyBalanceHandler : IPipeline<Result<UpdateUserBalanceState, string>, Result<decimal, string>>
{
    public const string InsufficientFundsError = "Insufficient funds";

    public Result<decimal, string> Execute(Result<UpdateUserBalanceState, string> input)
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

        state.UserBalance.Balance = newBalance;

        return Result.Ok(state.UserBalance.Balance);
    }
}
