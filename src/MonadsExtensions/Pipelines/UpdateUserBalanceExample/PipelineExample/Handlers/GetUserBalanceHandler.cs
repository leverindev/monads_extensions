using System.Linq;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.PipelineExample.Handlers
{
    public class GetUserBalanceHandler : IPipeline<Result<User, string>, Result<UserBalance, string>>
    {
        public Result<UserBalance, string> Execute(Result<User, string> input)
        {
            if (input.IsError(out var user, out var error))
            {
                return Result.Error(error);
            }

            var userBalance = Service.Database.UserBalances.FirstOrDefault(x => x.Id == user.Id);
            if (userBalance != null)
            {
                return Result.Ok(userBalance);
            }

            return Result.Error($"User balance with id {user.Id} not found");
        }
    }
}
