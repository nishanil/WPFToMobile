using Expenses.WPF.ViewModels;

namespace Expenses.WPF.Services
{
    public interface INavigationService
    {
        void ShowCharge(ChargeViewModel chargeViewModel);
        void ShowCharges();
        void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel);
        void ShowSubmittedExpenseReports();
        void ShowSavedExpenseReports();
        void ShowApprovedExpenseReports();
    }
}
