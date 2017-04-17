using MyExpenses.DataStores;
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
            Children.Add(new NavigationPage(new ExpensesPage())
            {
                Title = "Expenses",
                Icon = Device.OnPlatform("CreditCard.png", null, null)
            });
            Children.Add(new NavigationPage(new ReportsPage())
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

            await DependencyService.Get<IDataManager>().Init();

        }
    }
}
