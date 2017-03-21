using Expenses.WPF.ExpensesService;
using System.Threading.Tasks;

namespace Expenses.WPF.Services
{
    public interface IRepositoryService
    {
        Task<Employee> GetEmployeeAsync(string employeeAlias);
        
        Task<Charge[]> GetOutstandingChargesForEmployeeAsync(int employeeId);

        Task<Charge[]> GetChargesForExpenseReportAsync(int expenseReportId);
        
        Task<ExpenseReport[]> GetExpenseReportsForEmployeeAsync(int employeeId);

        Task<ExpenseReport[]> GetExpenseReportsForEmployeeByStatusAsync(int employeeId, ExpenseReportStatus status);

        Task<Charge> GetChargeAsync(int chargeId);

        Task<ExpenseReport> GetExpenseReportAsync(int expenseReportId);

        Task<int> CreateNewChargeAsync(Charge charge);

        Task<int> CreateNewExpenseReportAsync(ExpenseReport expenseReport);
        
        Task<int> UpdateChargeAsync(Charge charge);
        
        Task<int> UpdateExpenseReportAsync(ExpenseReport expenseReport);

        Task DeleteExpenseReportAsync(int expenseReportId);

        Task ResetDataAsync();
    }
}
