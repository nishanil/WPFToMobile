
using MyExpenses.Models;
using System.Threading.Tasks;

namespace Expenses.WPF.Services
{
    public interface IRepositoryService
    {
        Task<Employee> GetEmployeeAsync(string employeeAlias);
        
        Task<Charge[]> GetOutstandingChargesForEmployeeAsync(string employeeId);

        Task<Charge[]> GetChargesForExpenseReportAsync(string expenseReportId);
        
        Task<ExpenseReport[]> GetExpenseReportsForEmployeeAsync(string employeeId);

        Task<ExpenseReport[]> GetExpenseReportsForEmployeeByStatusAsync(string employeeId, ExpenseReportStatus status);

        Task<Charge> GetChargeAsync(string chargeId);

        Task<ExpenseReport> GetExpenseReportAsync(string expenseReportId);

        Task<string> CreateNewChargeAsync(Charge charge);

        Task<string> CreateNewExpenseReportAsync(ExpenseReport expenseReport);
        
        Task<string> UpdateChargeAsync(Charge charge);
        
        Task<string> UpdateExpenseReportAsync(ExpenseReport expenseReport);

        Task DeleteExpenseReportAsync(string expenseReportId);

        Task ResetDataAsync();
    }
}
