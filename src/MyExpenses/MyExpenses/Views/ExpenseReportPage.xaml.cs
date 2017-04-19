using System;

using MyExpenses.Models;
using MyExpenses.ViewModels;

using Xamarin.Forms;
using Expenses.WPF.ViewModels;
using Expenses.WPF.Services;
using System.Threading.Tasks;

namespace MyExpenses.Views
{
    public partial class ExpenseReportPage : ContentPage
    {
        ExpenseReportsViewModel viewModel;

        public ExpenseReportPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ExpenseReportsViewModel(
                DependencyService.Get<IServiceFactory>());
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ExpenseReportViewModel;
            if (item == null)
                return;

             await ShowExpenseReportAsync(item);


            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            ExpenseReportViewModel reportVM = new ExpenseReportViewModel(DependencyService.Get<IServiceFactory>())
            {
                Approver = "jhill",
                EmployeeId = App.EmployeeId,
                ExpenseReportId = null,
            };

            await this.ShowExpenseReportAsync(reportVM);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ExpenseReports.Count == 0)
                await viewModel.LoadAllExpenseReportsAsync();

            SortOptionsChanged += viewModel.SelectedReportStatusChanged;
            DependencyService.Get<NavigationService>().ShowSavedExpenseReportsRequested += ReportsPage_ShowSavedExpenseReportsRequested;
        }

        public async Task ShowExpenseReportAsync(ExpenseReportViewModel expenseReportViewModel)
        {
            await DependencyService.Get<IViewService>().ExecuteBusyActionAsync(
                async () =>
                {
                    var serviceFactory = DependencyService.Get<IServiceFactory>();
                    //TODO:
                    expenseReportViewModel.EmployeeId = App.EmployeeId;

                    var editReportVM = new EditExpenseReportViewModel(serviceFactory)
                    {
                        ExpenseReport = expenseReportViewModel,
                        
                    };
                    AddChargesViewModel addChargesVM = new AddChargesViewModel(serviceFactory);
                    await addChargesVM.LoadChargesAsync();
                    editReportVM.AddCharges = addChargesVM;

                    ExpenseReportChargesViewModel associatedChargesVM = new ExpenseReportChargesViewModel(serviceFactory);
                    await associatedChargesVM.LoadChargesAsync(expenseReportViewModel.ExpenseReportId);
                    editReportVM.AssociatedCharges = associatedChargesVM;

                    await Navigation?.PushAsync(new ExpenseReportDetailPage() { ViewModel = editReportVM }, true);
                });
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
            await DisplayAlert("Record Deleted", itemViewModel.Notes, "OK");
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
