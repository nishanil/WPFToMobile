
using MyExpenses.ViewModels;

using Xamarin.Forms;

namespace MyExpenses.Views
{
	public partial class ReportDetailPage : ContentPage
	{
		ItemDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ReportDetailPage()
        {
            InitializeComponent();
        }

        public ReportDetailPage(ItemDetailViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;
		}
	}
}
