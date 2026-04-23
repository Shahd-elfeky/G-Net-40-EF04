using System;
using System.Collections.Generic;

namespace Assignment_EF_04.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string NationalId { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HomeAddress { get; set; }
        public CustomerType Type { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new List<CustomerAccount>();
    }

    public enum CustomerType
    {
        Individual = 1,
        Business = 2
    }
}
