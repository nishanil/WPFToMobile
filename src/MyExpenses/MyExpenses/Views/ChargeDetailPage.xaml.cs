using System;

using MyExpenses.Models;

using Xamarin.Forms;
using MyExpenses.ViewModels;
using Expenses.WPF.ViewModels;
using Expenses.WPF.Services;

namespace MyExpenses.Views
{
	public partial class ChargeDetailPage : ContentPage
	{
        public ChargeViewModel ViewModel {
            get { return (ChargeViewModel)BindingContext; }
            set { BindingContext = value; }
        }

		public ChargeDetailPage()
		{
			InitializeComponent();



        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DependencyService.Get<NavigationService>().ShowChargesRequested += Save_Clicked;

        }

        protected override void OnDisappearing()
        {
            DependencyService.Get<NavigationService>().ShowChargesRequested -= Save_Clicked;

            base.OnDisappearing();
        }
        async void Save_Clicked(object sender, EventArgs e)
		{
			//MessagingCenter.Send(this, "AddItem", Item);
			await Navigation?.PopToRootAsync();
		}
	}
}