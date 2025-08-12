using System.Linq;
using System.Threading.Tasks;
using MonadsExtensions.Pipelines.Async;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample.Handlers
{
    public class GetUserAsyncHandler : IAsyncPipeline<long, Result<User, string>>
    {
        public async Task<Result<User, string>> ExecuteAsync(long userId)
        {
            await Task.Delay(1000); // Simulate async operation

            var user = PipelineExample.Service.Database.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return Result.Ok(user);
            }

            return Result.Error($"User with id {userId} not found");
        }
    }
}
