using System.Threading.Tasks;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Models;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample.Handlers
{
    public class ModifyBalanceAsyncHandler : IAsyncPipeline<Result<UpdateUserBalanceState, string>, Result<decimal, string>>
    {
        public async Task<Result<decimal, string>> ExecuteAsync(Result<UpdateUserBalanceState, string> input)
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

            // Simulate an asynchronous operation
            await Task.Delay(100);

            state.UserBalance.Balance = newBalance;

            return Result.Ok(state.UserBalance.Balance);
        }
    }
}
