using Expenses.WPF.Services;
using Expenses.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyExpenses.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseReportDetailPage : ContentPage
    {
        public EditExpenseReportViewModel ViewModel
        {
            get { return (EditExpenseReportViewModel)BindingContext; }
            set { BindingContext = value; }
        }

        public ExpenseReportDetailPage()
        {
            InitializeComponent();

        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            //await DisplayAlert("Selected", e.SelectedItem.ToString(), "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (multiPage != null)
            {
                var selectedCharges = multiPage.GetSelection();
                if (selectedCharges?.Count > 0)
                    foreach (var item in selectedCharges)
                    {
                        ViewModel.AddChargeToReportCommand.Execute(item);
                    }
                multiPage = null;
            }
        }

        SelectMultipleBasePage<ChargeViewModel> multiPage;
        private async void Button_Clicked(object sender, EventArgs e)
        {


            if (multiPage == null)
                multiPage = new SelectMultipleBasePage<ChargeViewModel>(ViewModel.
                    AddCharges.Charges.ToList())
                { Title = "Select Charges" };

            await Navigation.PushAsync(multiPage);
        }


        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var itemViewModel = mi.CommandParameter as ChargeViewModel;
            ViewModel.RemoveChargeFromReportCommand.Execute(itemViewModel);
            await DisplayAlert("Charge removed", itemViewModel.Description, "OK");
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            ViewModel.SaveReportCommand.Execute(null);
            await Navigation?.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (await DependencyService.Get<ViewService>().ConfirmAsync("Expense Report", "Are you sure you want to submit?"))
            {
                ViewModel.ExpenseReport.SubmitReportCommand.Execute(null);
                await Navigation?.PopAsync();
            }
        }
    }
}
