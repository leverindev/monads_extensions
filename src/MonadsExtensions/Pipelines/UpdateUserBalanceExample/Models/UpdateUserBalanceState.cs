using MonadsExtensions.Pipelines.UpdateUserBalanceExample.Database.Entities;

namespace MonadsExtensions.Pipelines.UpdateUserBalanceExample.Models
{
    public class UpdateUserBalanceState
    {
        public User User { get; set; }

        public UserBalance UserBalance { get; set; }

        public UserBalanceTransaction Transaction { get; set; }
    }
}
