using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Models;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.PipelineExample.Handlers
{
    public class ModifyBalanceHandler : IPipeline<Result<UpdateUserBalanceState, string>, Result<decimal, string>>
    {
        public Result<decimal, string> Execute(Result<UpdateUserBalanceState, string> input)
        {
            if (input.IsError(out var state, out var error))
            {
                return Result.Error(error);
            }

            var newBalance = state.UserBalance.Balance + state.Transaction.Amount;
            if (newBalance < 0)
            {
                return Result.Error("Insufficient funds");
            }

            state.UserBalance.Balance = newBalance;

            return Result.Ok(state.UserBalance.Balance);
        }
    }
}
