using System;

namespace Assignment_EF_04.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public string FullName { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
    }
}
