using System;
using System.Collections.Generic;

namespace Assignment_EF_04.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string Name { get; set; } = null!;
        public string BranchCode { get; set; } = null!;
        public string? Address { get; set; }
        public string? ContactPhone { get; set; }

        public Manager? Manager { get; set; }
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
