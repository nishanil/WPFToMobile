using Expenses.WPF.Services;
using Expenses.WPF.ViewModels;
using MyExpenses.DataStores;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ChargesPage : ContentPage
    {
        bool refreshRequired = false;

        ExpensesViewModel viewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0 || refreshRequired)
            {
                viewModel.RefreshDataCommand.Execute(null);
                refreshRequired = false;
            }
        }

       

        public ChargesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ExpensesViewModel();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await Navigation?.PushAsync(new ChargeDetailPage
            {
                ViewModel =
                new ChargeViewModel(e.SelectedItem as Charge,
                   DependencyService.Get<IServiceFactory>())
            }, true);
            //pushing into detail page hence refresh is required when back
            refreshRequired = true;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
           await Navigation?.PushAsync(new ChargeDetailPage
            {
                ViewModel =
                new ChargeViewModel(new Charge()
                {
                    Id = null,
                    ExpenseReportId = null,
                    EmployeeId = App.EmployeeId,
                    ExpenseDate = DateTime.Today,
                },
                   DependencyService.Get<IServiceFactory>())
            }, true);
            //pushing into detail page hence refresh is required when back
            refreshRequired = true;
        }
    }



}
