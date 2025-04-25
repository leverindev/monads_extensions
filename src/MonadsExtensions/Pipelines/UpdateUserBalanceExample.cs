using System;
using System.Collections.Generic;
using System.Linq;
using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Pipelines
{
    public class UpdateUserBalanceExample
    {
        public static readonly Database Database = new Database();

        public void UpdateUserBalance(UserBalanceTransaction transaction)
        {
            var pipeline = new GetUserHandler()
                .PipeTo(new GetUserBalanceHandler())
                .PipeTo(new ModifyBalanceHandler());

            var state = new UpdateUserBalanceState { Transaction = transaction };

            var result = pipeline.Execute(state);

            Console.WriteLine(
                result.IsValue(out var newBalance, out var error)
                    ? $"New balance: {newBalance}"
                    : $"Transaction error: {error}");
        }
    }

    public class GetUserHandler : IPipeline<UpdateUserBalanceState, Result<UpdateUserBalanceState, string>>
    {
        public Result<UpdateUserBalanceState, string> Execute(UpdateUserBalanceState state)
        {
            var user = UpdateUserBalanceExample.Database.Users.FirstOrDefault(x => x.Id == state.Transaction.UserId);
            if (user != null)
            {
                state.User = user;
                return Result.Ok(state);
            }

            return Result.Error($"User with id {state.Transaction.UserId} not found");
        }
    }

    public class GetUserBalanceHandler : IPipeline<Result<UpdateUserBalanceState, string>, Result<UpdateUserBalanceState, string>>
    {
        public Result<UpdateUserBalanceState, string> Execute(Result<UpdateUserBalanceState, string> input)
        {
            if (input.IsError(out var state, out var error))
            {
                return Result.Error(error);
            }

            var userBalance = UpdateUserBalanceExample.Database.UserBalances.FirstOrDefault(x => x.Id == state.User.Id);
            if (userBalance != null)
            {
                state.UserBalance = userBalance;
                return Result.Ok(state);
            }

            return Result.Error($"User balance with id {state.User.Id} not found");
        }
    }

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

    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class UserBalance
    {
        public long Id { get; set; }

        public decimal Balance { get; set; }
    }

    public class UserBalanceTransaction
    {
        public long UserId { get; set; }

        public decimal Amount { get; set; }
    }

    public class UpdateUserBalanceState
    {
        public User User { get; set; }

        public UserBalance UserBalance { get; set; }

        public UserBalanceTransaction Transaction { get; set; }
    }

    public class Database
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<UserBalance> UserBalances { get; set; } = new List<UserBalance>();

        public Database()
        {
            Users.Add(new User { Id = 1, Name = "John Doe" });
            Users.Add(new User { Id = 2, Name = "Jane Smith" });
            Users.Add(new User { Id = 3, Name = "Alice Johnson" });

            UserBalances.Add(new UserBalance { Id = 1, Balance = 100 });
            UserBalances.Add(new UserBalance { Id = 2, Balance = 200 });
            UserBalances.Add(new UserBalance { Id = 3, Balance = 300 });
        }
    }
}
