using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.OptionContainer;
using MonadsExtensions.Tests.Pipelines.AsynchronousTests.Handlers;
using MonadsExtensions.Tests.Pipelines.AsynchronousTests.Services;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.AsynchronousTests;

public class UpdateUserBalanceAsyncTests : OptionTestsBase
{
    private const int UserId = 1;
    private const int InvalidUserId = -1000;

    [Fact]
    public async Task EnoughFunds_UpdateBalance_ResultSuccessful()
    {
        // Arrange
        const decimal transactionValue = 100;
        const int transactionCount = 2;
        const decimal expectedValue = DatabaseContext.User1InitialBalance + transactionValue * transactionCount;

        var database = new DatabaseContext();
        var service = new UpdateUserBalanceAsyncService(database);

        // Act
        Result<decimal, string>? balanceResult = null;
        for (int i = 0; i < transactionCount; i++)
        {
            balanceResult = await service.UpdateUserBalanceAsync(new UserBalanceTransaction { UserId = UserId, Amount = transactionValue });
        }

        // Assert
        Assert.True(balanceResult.HasValue);
        Assert.Equal(expectedValue, balanceResult.Value.Value);
    }

    [Fact]
    public async Task InsufficientFunds_UpdateBalance_ErrorResult()
    {
        // Arrange
        const decimal transactionValue = -DatabaseContext.User1InitialBalance * 2;

        var database = new DatabaseContext();
        var service = new UpdateUserBalanceAsyncService(database);

        // Act
        var balanceResult = await service.UpdateUserBalanceAsync(new UserBalanceTransaction { UserId = UserId, Amount = transactionValue });

        // Assert
        Assert.True(balanceResult.IsError(out _, out _));
        Assert.Equal(ModifyBalanceAsyncHandler.InsufficientFundsError, balanceResult.Error);
    }

    [Fact]
    public async Task InvalidUserId_UpdateBalance_ErrorResult()
    {
        // Arrange
        const decimal transactionValue = 100;

        var database = new DatabaseContext();
        var service = new UpdateUserBalanceAsyncService(database);

        // Act
        var balanceResult = await service.UpdateUserBalanceAsync(new UserBalanceTransaction { UserId = InvalidUserId, Amount = transactionValue });

        // Assert
        Assert.True(balanceResult.IsError(out _, out _));
        Assert.Equal(GetUserAsyncHandler.UserNotFoundError, balanceResult.Error);
    }
}
