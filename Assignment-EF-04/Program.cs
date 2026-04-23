using System;
using System.Linq;
using Assignment_EF_04.Data;
using Assignment_EF_04.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_EF_04
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new BankDbContext();

            // Auto apply migrations
            context.Database.Migrate();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("======================");
                Console.WriteLine("    National Bank Management");
                Console.WriteLine("======================");
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer");
                Console.WriteLine("3) Update Account Status (Active / Closed)");
                Console.WriteLine("4) Remove an Account from a Customer");
                Console.WriteLine("5) List all Customers (with accounts)");
                Console.WriteLine("0) Exit");
                Console.WriteLine("=============");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();
                Console.WriteLine("===============");

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddCustomer(context);
                            break;
                        case "2":
                            OpenAccount(context);
                            break;
                        case "3":
                            UpdateAccountStatus(context);
                            break;
                        case "4":
                            RemoveAccount(context);
                            break;
                        case "5":
                            ListCustomers(context);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static void AddCustomer(BankDbContext context)
        {
            Console.WriteLine("Add New Customer\n");
            Console.Write("Full Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("National ID: ");
            string nationalId = Console.ReadLine() ?? "";

            Console.Write("Date of Birth (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dob))
            {
                Console.WriteLine("Invalid Date format.");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? "";

            Console.Write("Address: ");
            string address = Console.ReadLine() ?? "";

            Console.WriteLine("Customer Type:\n1) Individual\n2) Business");
            Console.Write("Choice: ");
            if (!Enum.TryParse(Console.ReadLine(), out CustomerType type) || !Enum.IsDefined(typeof(CustomerType), type))
            {
                Console.WriteLine("Invalid customer type.");
                return;
            }

            var customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = dob,
                EmailAddress = email,
                PhoneNumber = phone,
                HomeAddress = address,
                Type = type
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            Console.WriteLine($"\nCustomer created successfully. CustomerId = {customer.CustomerId}");
        }

        static void OpenAccount(BankDbContext context)
        {
            Console.WriteLine("Open New Account\n");

            Console.Write("Account Number: ");
            string accountNo = Console.ReadLine() ?? "";

            Console.WriteLine("Account Type:\n1) Savings\n2) Current\n3) Business");
            Console.Write("Choice: ");
            if (!Enum.TryParse(Console.ReadLine(), out AccountType accType) || !Enum.IsDefined(typeof(AccountType), accType))
            {
                Console.WriteLine("Invalid account type.");
                return;
            }

            Console.Write("Branch Code: ");
            string branchCode = Console.ReadLine() ?? "";

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer Id.");
                return;
            }

            Console.WriteLine("Ownership Role:\n1) Primary\n2) CoHolder");
            Console.Write("Choice: ");
            if (!Enum.TryParse(Console.ReadLine(), out OwnershipRole role) || !Enum.IsDefined(typeof(OwnershipRole), role))
            {
                Console.WriteLine("Invalid ownership role.");
                return;
            }

            Console.WriteLine($"Validating branch '{branchCode}' and customer #{customerId}...");

            var branch = context.Branches.FirstOrDefault(b => b.BranchCode == branchCode);
            if (branch == null)
            {
                Console.WriteLine("Branch not found.");
                return;
            }

            var customer = context.Customers.Find(customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            var account = context.Accounts.Find(accountNo);
            if (account == null)
            {
                account = new Account
                {
                    AccountNumber = accountNo,
                    Type = accType,
                    OpeningDate = DateTime.Now,
                    CurrentBalance = 0,
                    BranchId = branch.BranchId
                };
                context.Accounts.Add(account);
            }

            var customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accountNo,
                StartDate = DateTime.Now,
                OwnershipRole = role,
                Status = AccountStatus.Active
            };

            context.CustomerAccounts.Add(customerAccount);
            context.SaveChanges();

            Console.WriteLine($"\nAccount '{accountNo}' created and linked to customer {customerId} as {role} owner.");
        }

        static void UpdateAccountStatus(BankDbContext context)
        {
            Console.WriteLine("Update Account Status\n");

            Console.Write("Account Number: ");
            string accountNo = Console.ReadLine() ?? "";

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer Id.");
                return;
            }

            Console.WriteLine("New Status:\n1) Active\n2) Closed");
            Console.Write("Choice: ");
            if (!Enum.TryParse(Console.ReadLine(), out AccountStatus status) || !Enum.IsDefined(typeof(AccountStatus), status))
            {
                Console.WriteLine("Invalid status.");
                return;
            }

            var customerAcc = context.CustomerAccounts.FirstOrDefault(ca => ca.AccountNumber == accountNo && ca.CustomerId == customerId);
            if (customerAcc == null)
            {
                Console.WriteLine("Account link not found.");
                return;
            }

            customerAcc.Status = status;
            context.SaveChanges();

            Console.WriteLine($"\nStatus updated to {status}.");
        }

        static void RemoveAccount(BankDbContext context)
        {
            Console.WriteLine("Remove Account From Customer\n");

            Console.Write("Account Number: ");
            string accountNo = Console.ReadLine() ?? "";

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer Id.");
                return;
            }

            var customerAcc = context.CustomerAccounts.FirstOrDefault(ca => ca.AccountNumber == accountNo && ca.CustomerId == customerId);
            if (customerAcc == null)
            {
                Console.WriteLine("Ownership link not found.");
                return;
            }

            context.CustomerAccounts.Remove(customerAcc);
            Console.WriteLine("\nOwnership link deleted.");

            if (!context.CustomerAccounts.Any(ca => ca.AccountNumber == accountNo && ca.CustomerId != customerId))
            {
                var account = context.Accounts.Find(accountNo);
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    Console.WriteLine($"That was the last owner. Account '{accountNo}' was also removed.");
                }
            }

            context.SaveChanges();
        }

        static void ListCustomers(BankDbContext context)
        {
            Console.WriteLine("All Customers\n");
            var customers = context.Customers
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"# {c.CustomerId} {c.FullName} ({c.Type})");
                if (c.CustomerAccounts.Any())
                {
                    foreach (var ca in c.CustomerAccounts)
                    {
                        Console.WriteLine($"  {ca.AccountNumber} {ca.Account.Type} Balance: {ca.Account.CurrentBalance:N2} {ca.OwnershipRole} {ca.Status} @{ca.Account.Branch.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("  (no accounts)");
                }
                Console.WriteLine();
            }
        }
    }
}