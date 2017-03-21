using Expenses.WPF.Services;

namespace Expenses.WPF.ViewModels
{
    class ExpenseReportChargesViewModel : ChargesViewModel
    {
        public ExpenseReportChargesViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }
    }
}
