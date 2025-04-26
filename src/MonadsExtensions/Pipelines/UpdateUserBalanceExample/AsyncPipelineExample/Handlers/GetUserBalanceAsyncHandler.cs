using System.Linq;
using System.Threading.Tasks;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample.Handlers
{
    public class GetUserBalanceAsyncHandler : IAsyncPipeline<Result<User, string>, Result<UserBalance, string>>
    {
        public async Task<Result<UserBalance, string>> ExecuteAsync(Result<User, string> input)
        {
            if (input.IsError(out var user, out var error))
            {
                return Result.Error(error);
            }

            await Task.Delay(100); // Simulate async work

            var userBalance = PipelineExample.Service.Database.UserBalances.FirstOrDefault(x => x.Id == user.Id);
            if (userBalance != null)
            {
                return Result.Ok(userBalance);
            }

            return Result.Error($"User balance with id {user.Id} not found");
        }
    }
}
