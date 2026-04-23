using System;

namespace Assignment_EF_04.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }

        public string AccountNumber { get; set; } = null!;
        public Account Account { get; set; } = null!;
    }

    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2,
        Transfer = 3,
        Payment = 4
    }
}
