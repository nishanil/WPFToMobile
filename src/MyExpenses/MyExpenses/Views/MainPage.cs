using Expenses.WPF.Services;
using MyExpenses.DataStores;
using MyExpenses.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyExpenses.Views
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Children.Add(new NavigationPage(new ChargesPage())
            {
                Title = "Expenses",
                Icon = Device.OnPlatform("CreditCard.png", null, null)
            });
            Children.Add(new NavigationPage(new ExpenseReportPage())
            {
                Title = "Reports",
                Icon = Device.OnPlatform("Chart.png", null, null)
            });
            Children.Add(new NavigationPage(new AboutPage())
            {
                Title = "Settings",
                Icon = Device.OnPlatform("Settings.png", null, null)
            });
            Children.Add(new NavigationPage(new AboutPage())
            {
                Title = "Profile",
                Icon = Device.OnPlatform("Person.png", null, null)
            });

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var dataManager = DependencyService.Get<IDataManager>();
            await dataManager.Init();
            await dataManager.SyncAll();

        }
    }
}
