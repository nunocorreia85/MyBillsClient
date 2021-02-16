using MyBills.Domain.Entities;

namespace BlazorApp.Models
{
    public class AccountModel : Account
    {
        public AccountModel(Account account)
        {
            if (account == null) return;

            HasAccount = true;
            Balance = account.Balance;
            Closed = account.Closed;
            Id = account.Id;
            ExternalId = account.ExternalId;
            BankAccountNumber = account.BankAccountNumber;
        }

        public string Name { get; init; }
        public string PostalCode { get; init; }
        public string Country { get; init; }
        public bool HasAccount { get; init; }
        public string CreateOrUpdate => HasAccount ? "Update" : "Create";
        public string[] Emails { get; init; }
    }
}
