
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

        Task<int> CreateNewChargeAsync(Charge charge);

        Task<int> CreateNewExpenseReportAsync(ExpenseReport expenseReport);
        
        Task<int> UpdateChargeAsync(Charge charge);
        
        Task<int> UpdateExpenseReportAsync(ExpenseReport expenseReport);

        Task DeleteExpenseReportAsync(string expenseReportId);

        Task ResetDataAsync();
    }
}
