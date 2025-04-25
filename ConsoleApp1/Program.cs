using MonadsExtensions.Pipelines;

// new StringToNumberProcessorExample().Run();

var updateUserBalanceExample = new UpdateUserBalanceExample();

updateUserBalanceExample.UpdateUserBalance(new UserBalanceTransaction
{
    UserId = 1,
    Amount = 100,
});

updateUserBalanceExample.UpdateUserBalance(new UserBalanceTransaction
{
    UserId = 2,
    Amount = -500,
});

updateUserBalanceExample.UpdateUserBalance(new UserBalanceTransaction
{
    UserId = 10,
    Amount = 100,
});
