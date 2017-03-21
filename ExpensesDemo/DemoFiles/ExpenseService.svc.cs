using Expenses.Data;
using Expenses.Data.CommunicationModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Security.Claims;
using System.Net;

namespace Expenses.Wcf
{
    public class ExpenseService : IExpenseService
    {
        private bool AuthorizationCheck()
        {
            if (ClaimsPrincipal.Current.HasClaim("http://schemas.microsoft.com/identity/claims/scope", "user_impersonation"))
            {
                return true;
            }
            else
            {
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }

        public Employee GetEmployee(string employeeAlias)
        {
            AuthorizationCheck();

            //Claim subject = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name);

            var repo = new ExpensesRepository();
            return new Employee(repo.GetEmployee(employeeAlias));
        }

        public ExpenseReport GetExpenseReport(int expenseReportId)
        {
            AuthorizationCheck();

            var repo = new ExpensesRepository();
            return new ExpenseReport(repo.GetExpenseReport(expenseReportId));
        }

        public Charge GetCharge(int chargeId)
        {
            AuthorizationCheck();

            var repo = new ExpensesRepository();
            return new Charge(repo.GetCharge(chargeId));
        }

        public ICollection<Charge> GetOutstandingChargesForEmployee(int employeeId)
        {
            AuthorizationCheck();

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
            AuthorizationCheck();

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
            AuthorizationCheck();

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
            AuthorizationCheck();

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
            AuthorizationCheck();

            var repo = new ExpensesRepository();
            charge.ChargeId = 0;
            var dbCharge = repo.SaveCharge(new DbCharge(charge));

            return dbCharge.ChargeId;
        }

        public int CreateNewExpenseReport(ExpenseReport expenseReport)
        {
            AuthorizationCheck();

            var repo = new ExpensesRepository();
            expenseReport.ExpenseReportId = 0;
            var dbExpenseReport = repo.SaveExpenseReport(new DbExpenseReport(expenseReport));

            return dbExpenseReport.ExpenseReportId;
        }

        public int UpdateCharge(Charge charge)
        {
            AuthorizationCheck();

            var repo = new ExpensesRepository();
            var dbCharge = repo.SaveCharge(new DbCharge(charge));

            return dbCharge.ChargeId;
        }

        public int UpdateExpenseReport(ExpenseReport expenseReport)
        {
            AuthorizationCheck();

            var repo = new ExpensesRepository();
            var dbExpenseReport = repo.SaveExpenseReport(new DbExpenseReport(expenseReport));

            return dbExpenseReport.ExpenseReportId;
        }

        public void DeleteExpenseReport(int expenseReportId)
        {
            AuthorizationCheck();

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
