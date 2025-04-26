using System.Collections.Generic;
using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database
{
    public class DatabaseContext
    {
        public List<User> Users { get; set; } = new List<User>();

        public List<UserBalance> UserBalances { get; set; } = new List<UserBalance>();

        public DatabaseContext()
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
