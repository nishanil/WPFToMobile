using System;

using MyExpenses.Models;

using Xamarin.Forms;

namespace MyExpenses.Views
{
	public partial class ExpenseDetailPage : ContentPage
	{
		public Item Item { get; set; }

		public ExpenseDetailPage()
		{
			InitializeComponent();

			Item = new Item
			{
				Text = "Item name",
				Description = "This is a nice description"
			};

			BindingContext = this;
		}

		async void Save_Clicked(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, "AddItem", Item);
			await Navigation.PopToRootAsync();
		}
	}
}