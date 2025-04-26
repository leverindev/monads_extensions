using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;

// new StringToNumberProcessorExample().Run();

var updateUserBalanceExample = new MonadsExtensions.Pipelines.UpdateUserBalanceExample.AsyncPipelineExample.AsyncService();

await updateUserBalanceExample.UpdateUserBalanceAsync(new UserBalanceTransaction
{
    UserId = 1,
    Amount = 100,
});

await updateUserBalanceExample.UpdateUserBalanceAsync(new UserBalanceTransaction
{
    UserId = 2,
    Amount = -500,
});

await updateUserBalanceExample.UpdateUserBalanceAsync(new UserBalanceTransaction
{
    UserId = 10,
    Amount = 100,
});
