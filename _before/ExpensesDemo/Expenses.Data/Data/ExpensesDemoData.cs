using System;
using System.Collections.Generic;

namespace Expenses.Data
{
    public static class ExpensesDemoData
    {
        public const string DefaultEmployeeAlias = "kimakers";

        public static DbEmployee CreateNewDemoEmployee(string alias)
        {
            var repository = new ExpensesRepository();
            string managerAlias = "manager";

            if (string.Compare(alias, "rogreen", true) != 0)
            {
                managerAlias = "rogreen";
            }

            DbEmployee employee =
                new DbEmployee()
                {
                    Alias = alias,
                    Manager = managerAlias,
                    Name = "New Employee"
                };

            employee = repository.SaveEmployee(employee);

            List<DbCharge> charges = new List<DbCharge>();

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                BilledAmount = 200M,
                Description = "REF# 27438948",
                ExpenseDate = DateTime.Today.AddDays(-45),
                Location = "San Francisco, CA",
                Merchant = "Northwind Inn",
                Notes = string.Empty,
                TransactionAmount = 200M
            });

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                BilledAmount = 40,
                Description = "REF# 77384751",
                ExpenseDate = DateTime.Today.AddDays(-20),
                Location = "Seattle, WA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                TransactionAmount = 40
            });

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                BilledAmount = 67,
                Description = "REF# 33748563",
                ExpenseDate = DateTime.Today.AddDays(-8),
                Location = "Seattle, WA",
                Merchant = "Fourth Coffee",
                Notes = string.Empty,
                TransactionAmount = 12
            });

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                BilledAmount = 17,
                Description = "REF# 33748876",
                ExpenseDate = DateTime.Today.AddDays(-4),
                Location = "Seattle, WA",
                Merchant = "Fourth Coffee",
                Notes = string.Empty,
                TransactionAmount = 15
            });


            repository.SaveExpenseReport(new DbExpenseReport()
            {
                EmployeeId = employee.EmployeeId,
                Amount = 640M,
                Approver = managerAlias,
                CostCenter = 50992,
                DateSubmitted = DateTime.Today.AddDays(-7),
                Notes = (managerAlias == "rogreen") ? "Kim Akers" : "Visit to Blue Yonder Airlines",
                Status = DbExpenseReportStatus.Saved
            });

            DbExpenseReport report = new DbExpenseReport()
            {
                EmployeeId = employee.EmployeeId,
                Amount = 450M,
                Approver = managerAlias,
                CostCenter = 50992,
                DateSubmitted = DateTime.Today.AddDays(-7),
                Notes = (managerAlias == "rogreen") ? "Kim Akers" : "Visit to Tailspin Toys",
                Status = DbExpenseReportStatus.Saved
            };
            report = repository.SaveExpenseReport(report);

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = report.ExpenseReportId,
                BilledAmount = 350M,
                Description = "Airfare to San Francisco",
                ExpenseDate = DateTime.Today.AddDays(-60),
                Location = "Chicago, IL",
                Merchant = "Blue Yonder Airlines",
                Notes = string.Empty,
                TransactionAmount = 350M
            });

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = report.ExpenseReportId,
                BilledAmount = 50,
                Description = "Cab from airport",
                ExpenseDate = DateTime.Today.AddDays(-45),
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                TransactionAmount = 50
            });

            repository.SaveCharge(new DbCharge()
            {
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = report.ExpenseReportId,
                BilledAmount = 50,
                Description = "Cab to airport",
                ExpenseDate = DateTime.Today.AddDays(-45),
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                TransactionAmount = 50
            });

            // Add a year of every other month customer visits
            int x = -75;
            for (int i = 1; i <= 6; i++)
            {
                report = new DbExpenseReport()
                {
                    EmployeeId = employee.EmployeeId,
                    Amount = 850M,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = DateTime.Today.AddDays(x - 5),
                    DateResolved = DateTime.Today.AddDays(x),
                    Notes = "Visit to Tailspin Toys",
                    Status = DbExpenseReportStatus.Approved,
                };
                repository.SaveExpenseReport(report);

                repository.SaveCharge(new DbCharge()
                {
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = report.ExpenseReportId,
                    BilledAmount = 350M,
                    Description = "Airfare to Chicago",
                    ExpenseDate = DateTime.Today.AddDays(x - 15),
                    Location = "Chicago, IL",
                    Merchant = "Blue Yonder Airlines",
                    Notes = string.Empty,
                    TransactionAmount = 350M
                });

                repository.SaveCharge(new DbCharge()
                {
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = report.ExpenseReportId,
                    BilledAmount = 50M,
                    Description = "Cab from airport",
                    ExpenseDate = DateTime.Today.AddDays(x - 5),
                    Location = "Chicago, IL",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    TransactionAmount = 50M
                });

                repository.SaveCharge(new DbCharge()
                {
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = report.ExpenseReportId,
                    BilledAmount = 50M,
                    Description = "Cab to airport",
                    ExpenseDate = DateTime.Today.AddDays(x - 3),
                    Location = "Chicago, IL",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    TransactionAmount = 50M
                });

                repository.SaveCharge(new DbCharge()
                {
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = report.ExpenseReportId,
                    BilledAmount = 400M,
                    Description = "2 nights hotel",
                    ExpenseDate = DateTime.Today.AddDays(x - 3),
                    Location = "Chicago, IL",
                    Merchant = "Northwind Inn",
                    Notes = string.Empty,
                    TransactionAmount = 400M
                });

                x -= 60;
            }

            // Add 18 months of cell phone charges
            x = -30;
            for (int i = 1; i <= 18; i++)
            {
                report = new DbExpenseReport()
                {
                    EmployeeId = employee.EmployeeId,
                    Amount = 850M,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = DateTime.Today.AddDays(x - 5),
                    DateResolved = DateTime.Today.AddDays(x),
                    Notes = "Last month's cell phone",
                    Status = DbExpenseReportStatus.Approved,
                };
                repository.SaveExpenseReport(report);

                repository.SaveCharge(new DbCharge()
                {
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = report.ExpenseReportId,
                    BilledAmount = 50M,
                    Description = "Cell phone bill",
                    ExpenseDate = DateTime.Today.AddDays(x - 10),
                    Location = "Seattle, WA",
                    Merchant = "The Phone Company",
                    Notes = string.Empty,
                    TransactionAmount = 50M
                });

                x -= 30;
            }

            return repository.GetEmployee(employee.EmployeeId);
        }

        /// <summary>
        /// Cleans up database by removing all records.
        /// </summary>
        public static void CleanRepository()
        {
            var repository = new ExpensesRepository();
            repository.CleanRepository();
        }
    }
}
