using Expenses.WPF.Services;

namespace Expenses.WPF.ViewModels
{
    public class ExpenseReportChargesViewModel : ChargesViewModel
    {
        public ExpenseReportChargesViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }
    }
}
