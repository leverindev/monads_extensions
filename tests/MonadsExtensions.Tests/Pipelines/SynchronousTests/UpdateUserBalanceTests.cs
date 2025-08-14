using MonadsExtensions.ResultContainer;
using MonadsExtensions.Tests.OptionContainer;
using MonadsExtensions.Tests.Pipelines.Database;
using MonadsExtensions.Tests.Pipelines.Database.Entities;
using MonadsExtensions.Tests.Pipelines.SynchronousTests.Handlers;
using MonadsExtensions.Tests.Pipelines.SynchronousTests.Services;

namespace MonadsExtensions.Tests.Pipelines.SynchronousTests;

public class UpdateUserBalanceTests : OptionTestsBase
{
    private const int UserId = 1;
    private const int InvalidUserId = -1000;

    [Fact]
    public void EnoughFunds_UpdateBalance_ResultSuccessful()
    {
        // Arrange
        const decimal transactionValue = 100;
        const int transactionCount = 2;
        const decimal expectedValue = DatabaseContext.User1InitialBalance + transactionValue * transactionCount;

        var database = new DatabaseContext();
        var service = new UpdateUserBalanceService(database);

        // Act
        Result<decimal, string>? balanceResult = null;
        for (int i = 0; i < transactionCount; i++)
        {
            balanceResult = service.UpdateUserBalance(new UserBalanceTransaction { UserId = UserId, Amount = transactionValue });
        }

        // Assert
        Assert.True(balanceResult.HasValue);
        Assert.Equal(expectedValue, balanceResult.Value.Value);
    }

    [Fact]
    public void InsufficientFunds_UpdateBalance_ErrorResult()
    {
        // Arrange
        const decimal transactionValue = -DatabaseContext.User1InitialBalance * 2;

        var database = new DatabaseContext();
        var service = new UpdateUserBalanceService(database);

        // Act
        var balanceResult = service.UpdateUserBalance(new UserBalanceTransaction { UserId = UserId, Amount = transactionValue });

        // Assert
        Assert.True(balanceResult.IsError(out _, out _));
        Assert.Equal(ModifyBalanceHandler.InsufficientFundsError, balanceResult.Error);
    }

    [Fact]
    public void InvalidUserId_UpdateBalance_ErrorResult()
    {
        // Arrange
        const decimal transactionValue = 100;

        var database = new DatabaseContext();
        var service = new UpdateUserBalanceService(database);

        // Act
        var balanceResult = service.UpdateUserBalance(new UserBalanceTransaction { UserId = InvalidUserId, Amount = transactionValue });

        // Assert
        Assert.True(balanceResult.IsError(out _, out _));
        Assert.Equal(GetUserHandler.UserNotFoundError, balanceResult.Error);
    }
}
