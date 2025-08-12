using MonadsExtensions.Pipelines.Sync;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Models;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample.Handlers
{
    public class UpdateStateHandler<T> : IPipeline<Result<T, string>, Result<T, string>>
    {
        private readonly UpdateUserBalanceState _state;

        public UpdateStateHandler(UpdateUserBalanceState state)
        {
            _state = state;
        }

        public Result<T, string> Execute(Result<T, string> input)
        {
            if (input.IsError(out var value, out var error))
            {
                return Result.Error(error);
            }

            switch (value)
            {
                case User user:
                    _state.User = user;
                    break;
                case UserBalance userBalance:
                    _state.UserBalance = userBalance;
                    break;
            }

            return input;
        }
    }
}
