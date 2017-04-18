using System;

using MyExpenses.Models;
using MyExpenses.ViewModels;

using Xamarin.Forms;
using Expenses.WPF.ViewModels;
using Expenses.WPF.Services;

namespace MyExpenses.Views
{
	public partial class ReportsPage : ContentPage
	{
        ExpenseReportsViewModel viewModel;

		public ReportsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ExpenseReportsViewModel(
                DependencyService.Get<IServiceFactory>());
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as Item;
			if (item == null)
				return;

			await Navigation.PushAsync(new ReportsDetailPage());

			// Manually deselect item
			ItemsListView.SelectedItem = null;
		}

		async void AddItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ReportsDetailPage());
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

            if (viewModel.ExpenseReports.Count == 0)
                await viewModel.LoadAllExpenseReportsAsync();

            SortOptionsChanged += viewModel.SelectedReportStatusChanged;
            DependencyService.Get<NavigationService>().ShowSavedExpenseReportsRequested += ReportsPage_ShowSavedExpenseReportsRequested;
        }

        private void ReportsPage_ShowSavedExpenseReportsRequested(object sender, EventArgs e)
        {
            sortOptions.SelectedIndex = 0;
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var itemViewModel = mi.CommandParameter as ExpenseReportViewModel;
            await itemViewModel.DeleteAsync();
            await DisplayAlert("Record Deleted",itemViewModel.Notes, "OK");
        }

        protected override void OnDisappearing()
        {
            SortOptionsChanged -= viewModel.SelectedReportStatusChanged;
            DependencyService.Get<NavigationService>().ShowSavedExpenseReportsRequested -= ReportsPage_ShowSavedExpenseReportsRequested;

            base.OnDisappearing();
        }

        public event EventHandler<string> SortOptionsChanged;
        private void Options_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = sortOptions.Items[sortOptions.SelectedIndex];
            SortOptionsChanged?.Invoke(this, selectedItem);
        }
    }
}
