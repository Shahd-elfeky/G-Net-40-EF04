using Assignment_EF_04.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Assignment_EF_04.Data
{
    public class BankDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BankDbEf04;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Branch Config
            modelBuilder.Entity<Branch>()
                .HasIndex(b => b.BranchCode)
                .IsUnique();

            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Manager)
                .WithOne(m => m.Branch)
                .HasForeignKey<Manager>(m => m.BranchId);

            // Account Config
            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountNumber);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Branch)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // CustomerAccount Config
            modelBuilder.Entity<CustomerAccount>()
                .HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

            modelBuilder.Entity<CustomerAccount>()
                .HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerAccount>()
                .HasOne(ca => ca.Account)
                .WithMany(a => a.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction Config
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data
            modelBuilder.Entity<Branch>().HasData(
                new Branch { BranchId = 1, Name = "Cairo Main Branch", BranchCode = "CAI-01", Address = "123 Cairo St.", ContactPhone = "01000000001" },
                new Branch { BranchId = 2, Name = "Alexandria Branch", BranchCode = "ALX-01", Address = "456 Alex St.", ContactPhone = "01000000002" }
            );

            modelBuilder.Entity<Manager>().HasData(
                new Manager { ManagerId = 1, FullName = "Ahmed Ali", EmailAddress = "ahmed.ali@bank.com", PhoneNumber = "01011111111", HireDate = new DateTime(2020, 1, 1), BranchId = 1 },
                new Manager { ManagerId = 2, FullName = "Mona Adel", EmailAddress = "mona.adel@bank.com", PhoneNumber = "01022222222", HireDate = new DateTime(2021, 5, 1), BranchId = 2 }
            );
        }
    }
}
