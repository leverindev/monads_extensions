using MonadsExtensions.Tests.Pipelines.Database.Entities;

namespace MonadsExtensions.Tests.Pipelines.Database;

public class DatabaseContext
{
    public const decimal User1InitialBalance = 100;
    public const decimal User2InitialBalance = 200;
    public const decimal User3InitialBalance = 300;

    public List<User> Users { get; set; } = new ();

    public List<UserBalance> UserBalances { get; set; } = new ();

    public DatabaseContext()
    {
        Users.Add(new User { Id = 1, Name = "John" });
        Users.Add(new User { Id = 2, Name = "Jane" });
        Users.Add(new User { Id = 3, Name = "Alice" });

        UserBalances.Add(new UserBalance { Id = 1, Balance = User1InitialBalance });
        UserBalances.Add(new UserBalance { Id = 2, Balance = User2InitialBalance });
        UserBalances.Add(new UserBalance { Id = 3, Balance = User3InitialBalance });
    }
}
