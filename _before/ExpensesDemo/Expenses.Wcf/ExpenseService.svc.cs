using Expenses.Data;
using Expenses.Data.CommunicationModels;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Wcf
{
    public class ExpenseService : IExpenseService
    {
        public Employee GetEmployee(string employeeAlias)
        {
            var repo = new ExpensesRepository();
            return new Employee(repo.GetEmployee(employeeAlias));
        }

        public ExpenseReport GetExpenseReport(int expenseReportId)
        {
            var repo = new ExpensesRepository();
            return new ExpenseReport(repo.GetExpenseReport(expenseReportId));
        }

        public Charge GetCharge(int chargeId)
        {
            var repo = new ExpensesRepository();
            return new Charge(repo.GetCharge(chargeId));
        }

        public ICollection<Charge> GetOutstandingChargesForEmployee(int employeeId)
        {
            var repo = new ExpensesRepository();
            var employee = repo.GetEmployee(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Unknown employeeId.");
            }

            var dbChargeCollection = employee.Charges;
            return dbChargeCollection
                .Where((c) => !c.ExpenseReportId.HasValue)
                .Select((dbCharge) => new Charge(dbCharge))
                .ToList();
        }

        public ICollection<Charge> GetChargesForExpenseReport(int expenseReportId)
        {
            var repo = new ExpensesRepository();
            var expenseReport = repo.GetExpenseReport(expenseReportId);
            if (expenseReport == null)
            {
                throw new KeyNotFoundException("Unknown expenseReportId.");
            }
            var dbChargeCollection = expenseReport.Charges;
            return dbChargeCollection
                .Select((dbCharge) => new Charge(dbCharge))
                .ToList();
        }

        public ICollection<ExpenseReport> GetExpenseReportsForEmployee(int employeeId)
        {
            var repo = new ExpensesRepository();
            var employee = repo.GetEmployee(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Unknown employeeId.");
            }

            var dbExpenseReportCollection = employee.ExpenseReports;
            return dbExpenseReportCollection
                .Select((dbExpenseReport) => new ExpenseReport(dbExpenseReport))
                .ToList();
        }

        public ICollection<ExpenseReport> GetExpenseReportsForEmployeeByStatus(int employeeId, ExpenseReportStatus status)
        {
            var repo = new ExpensesRepository();
            var employee = repo.GetEmployee(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Unknown employeeId.");
            }

            var dbExpenseReportCollection = employee.ExpenseReports;
            return dbExpenseReportCollection
                .Where((dbExpenseReport) => dbExpenseReport.Status == (DbExpenseReportStatus)status)
                .Select((dbExpenseReport) => new ExpenseReport(dbExpenseReport))
                .ToList();
        }

        public int CreateNewCharge(Charge charge)
        {
            var repo = new ExpensesRepository();
            charge.ChargeId = 0;
            var dbCharge = repo.SaveCharge(new DbCharge(charge));

            return dbCharge.ChargeId;
        }

        public int CreateNewExpenseReport(ExpenseReport expenseReport)
        {
            var repo = new ExpensesRepository();
            expenseReport.ExpenseReportId = 0;
            var dbExpenseReport = repo.SaveExpenseReport(new DbExpenseReport(expenseReport));

            return dbExpenseReport.ExpenseReportId;
        }

        public int UpdateCharge(Charge charge)
        {
            var repo = new ExpensesRepository();
            var dbCharge = repo.SaveCharge(new DbCharge(charge));

            return dbCharge.ChargeId;
        }

        public int UpdateExpenseReport(ExpenseReport expenseReport)
        {
            var repo = new ExpensesRepository();
            var dbExpenseReport = repo.SaveExpenseReport(new DbExpenseReport(expenseReport));

            return dbExpenseReport.ExpenseReportId;
        }

        public void DeleteExpenseReport(int expenseReportId)
        {
            var repo = new ExpensesRepository();
            repo.DeleteExpenseReport(expenseReportId);
        }

        public void ResetData()
        {
            ExpensesDemoData.CleanRepository();
            ExpensesDemoData.CreateNewDemoEmployee(ExpensesDemoData.DefaultEmployeeAlias);
        }
    }
}
