using System;

namespace Assignment_EF_04.Models
{
    public class CustomerAccount
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string AccountNumber { get; set; } = null!;
        public Account Account { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public OwnershipRole OwnershipRole { get; set; }
        public AccountStatus Status { get; set; }
    }

    public enum OwnershipRole
    {
        Primary = 1,
        CoHolder = 2
    }

    public enum AccountStatus
    {
        Active = 1,
        Closed = 2
    }
}
