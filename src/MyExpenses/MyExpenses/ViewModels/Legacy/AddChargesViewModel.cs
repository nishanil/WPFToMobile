using Expenses.WPF.Services;

namespace Expenses.WPF.ViewModels
{
    public class AddChargesViewModel : ChargesViewModel
    {
        public AddChargesViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }
    }
}
