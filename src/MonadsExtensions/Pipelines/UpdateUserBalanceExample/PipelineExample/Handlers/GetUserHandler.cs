using System.Linq;
using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.PipelineExample.Handlers
{
    public class GetUserHandler : IPipeline<long, Result<User, string>>
    {
        public Result<User, string> Execute(long userId)
        {
            var user = Service.Database.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return Result.Ok(user);
            }

            return Result.Error($"User with id {userId} not found");
        }
    }
}
