using System;
using System.Collections.Generic;

namespace Assignment_EF_04.Models
{
    public class Account
    {
        public string AccountNumber { get; set; } = null!;
        public AccountType Type { get; set; }
        public DateTime OpeningDate { get; set; }
        public decimal CurrentBalance { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public enum AccountType
    {
        Savings = 1,
        Current = 2,
        Business = 3
    }
}
