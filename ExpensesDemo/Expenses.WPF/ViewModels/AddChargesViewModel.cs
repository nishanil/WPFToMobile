using Expenses.WPF.Services;

namespace Expenses.WPF.ViewModels
{
    class AddChargesViewModel : ChargesViewModel
    {
        public AddChargesViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }
    }
}
