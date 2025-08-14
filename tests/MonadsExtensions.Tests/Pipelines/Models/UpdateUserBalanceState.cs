using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.Models;

public class UpdateUserBalanceState
{
    public required UserBalanceTransaction Transaction { get; init; }

    public User? User { get; set; }

    public UserBalance? UserBalance { get; set; }
}
