using MyExpenses.MobileAppService.DataObjects;
using MyExpenses.MobileAppService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MyExpenses.MobileAppService.DataObjects.ExpenseReport;

namespace MyExpenses.MobileAppService
{
    public class ExpensesDemoData
    {
        public const string DefaultEmployeeAlias = "nanil";

        public static Employee CreateNewDemoEmployee(string alias, MyExpenseContext context)
        {
            string managerAlias = "manager";

            if (string.Compare(alias, "rogreen", true) != 0)
            {
                managerAlias = "rogreen";
            }

            var employee =
                new Employee()
                {
                    Id = Guid.NewGuid().ToString(),
                    Alias = alias,
                    Manager = managerAlias,
                    Name = "New Employee"
                };

            context.Set<Employee>().Add(employee);
            context.SaveChanges();

            List<Charge> charges = new List<Charge>();

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                BilledAmount = 200M,
                Description = "REF# 27438948",
                ExpenseDate = DateTime.Today.AddDays(-45),
                Location = "San Francisco, CA",
                Merchant = "Northwind Inn",
                Notes = string.Empty,
                ChargeCategory = Charge.Category.Hotel,
                TransactionAmount = 200M
            });

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                BilledAmount = 40,
                Description = "REF# 77384751",
                ExpenseDate = DateTime.Today.AddDays(-20),
                Location = "Seattle, WA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ChargeCategory = Charge.Category.Taxi,
                TransactionAmount = 40
            });

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                BilledAmount = 67,
                Description = "REF# 33748563",
                ExpenseDate = DateTime.Today.AddDays(-8),
                Location = "Seattle, WA",
                Merchant = "Fourth Coffee",
                ChargeCategory = Charge.Category.Meal,
                Notes = string.Empty,
                TransactionAmount = 12
            });

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                BilledAmount = 17,
                Description = "REF# 33748876",
                ExpenseDate = DateTime.Today.AddDays(-4),
                Location = "Seattle, WA",
                Merchant = "Fourth Coffee",
                ChargeCategory = Charge.Category.Meal,
                Notes = string.Empty,
                TransactionAmount = 15
            });

            context.SaveChanges();

            context.Set<ExpenseReport>().Add(new ExpenseReport()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                Amount = 640M,
                Approver = managerAlias,
                CostCenter = 50992,
                DateSubmitted = DateTime.Today.AddDays(-7),
                Notes = (managerAlias == "rogreen") ? "Kim Akers" : "Visit to Blue Yonder Airlines",
                Status = ExpenseReportStatus.Saved
            });

            ExpenseReport report = new ExpenseReport()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                Amount = 450M,
                Approver = managerAlias,
                CostCenter = 50992,
                DateSubmitted = DateTime.Today.AddDays(-7),
                Notes = (managerAlias == "rogreen") ? "Kim Akers" : "Visit to Tailspin Toys",
                Status = ExpenseReportStatus.Saved
            };
            context.Set<ExpenseReport>().Add(report);

            context.SaveChanges();

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                ExpenseReportId = report.Id,
                BilledAmount = 350M,
                Description = "Airfare to San Francisco",
                ExpenseDate = DateTime.Today.AddDays(-60),
                Location = "Chicago, IL",
                Merchant = "Blue Yonder Airlines",
                ChargeCategory = Charge.Category.Flight,
                Notes = string.Empty,
                TransactionAmount = 350M
            });

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                ExpenseReportId = report.Id,
                BilledAmount = 50,
                Description = "Cab from airport",
                ExpenseDate = DateTime.Today.AddDays(-45),
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                ChargeCategory = Charge.Category.Taxi,
                Notes = string.Empty,
                TransactionAmount = 50
            });

            context.Set<Charge>().Add(new Charge()
            {
                Id = Guid.NewGuid().ToString(),

                EmployeeId = employee.Id,
                ExpenseReportId = report.Id,
                BilledAmount = 50,
                Description = "Cab to airport",
                ExpenseDate = DateTime.Today.AddDays(-45),
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                ChargeCategory = Charge.Category.Taxi,
                Notes = string.Empty,
                TransactionAmount = 50
            });

            context.SaveChanges();

            // Add a year of every other month customer visits
            int x = -75;
            for (int i = 1; i <= 6; i++)
            {
                report = new ExpenseReport()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    Amount = 850M,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = DateTime.Today.AddDays(x - 5),
                    DateResolved = DateTime.Today.AddDays(x),
                    Notes = "Visit to Tailspin Toys",
                    Status = ExpenseReportStatus.Approved,
                };
                context.Set<ExpenseReport>().Add(report);

                context.SaveChanges();

                context.Set<Charge>().Add(new Charge()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    ExpenseReportId = report.Id,
                    BilledAmount = 350M,
                    Description = "Airfare to Chicago",
                    ExpenseDate = DateTime.Today.AddDays(x - 15),
                    Location = "Chicago, IL",
                    Merchant = "Blue Yonder Airlines",
                    ChargeCategory = Charge.Category.Flight,
                    Notes = string.Empty,
                    TransactionAmount = 350M
                });

                context.Set<Charge>().Add(new Charge()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    ExpenseReportId = report.Id,
                    BilledAmount = 50M,
                    Description = "Cab from airport",
                    ExpenseDate = DateTime.Today.AddDays(x - 5),
                    Location = "Chicago, IL",
                    Merchant = "Contoso Taxi",
                    ChargeCategory = Charge.Category.Taxi,
                    Notes = string.Empty,
                    TransactionAmount = 50M
                });

                context.Set<Charge>().Add(new Charge()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    ExpenseReportId = report.Id,
                    BilledAmount = 50M,
                    Description = "Cab to airport",
                    ExpenseDate = DateTime.Today.AddDays(x - 3),
                    Location = "Chicago, IL",
                    Merchant = "Contoso Taxi",
                    ChargeCategory = Charge.Category.Taxi,
                    Notes = string.Empty,
                    TransactionAmount = 50M
                });

                context.Set<Charge>().Add(new Charge()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    ExpenseReportId = report.Id,
                    BilledAmount = 400M,
                    Description = "2 nights hotel",
                    ExpenseDate = DateTime.Today.AddDays(x - 3),
                    Location = "Chicago, IL",
                    Merchant = "Northwind Inn",
                    ChargeCategory = Charge.Category.Hotel,
                    Notes = string.Empty,
                    TransactionAmount = 400M
                });
                context.SaveChanges();

                x -= 60;
            }

            // Add 18 months of cell phone charges
            x = -30;
            for (int i = 1; i <= 18; i++)
            {
                report = new ExpenseReport()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    Amount = 850M,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = DateTime.Today.AddDays(x - 5),
                    DateResolved = DateTime.Today.AddDays(x),
                    Notes = "Last month's cell phone",
                    Status = ExpenseReportStatus.Approved,
                };
                context.Set<ExpenseReport>().Add(report);
                context.SaveChanges();

                context.Set<Charge>().Add(new Charge()
                {
                    Id = Guid.NewGuid().ToString(),

                    EmployeeId = employee.Id,
                    ExpenseReportId = report.Id,
                    BilledAmount = 50M,
                    Description = "Cell phone bill",
                    ExpenseDate = DateTime.Today.AddDays(x - 10),
                    Location = "Seattle, WA",
                    Merchant = "The Phone Company",
                    Notes = string.Empty,
                    ChargeCategory = Charge.Category.Other,

                    TransactionAmount = 50M
                });
                context.SaveChanges();

                x -= 30;
            }

            return employee;
        }
    }
}