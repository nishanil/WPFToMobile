using System;
using System.Diagnostics;
using System.Threading.Tasks;

using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Views;

using Xamarin.Forms;

namespace MyExpenses.ViewModels
{
	public class ItemsViewModel : BaseViewModel
	{
		public ObservableRangeCollection<ExpenseReport> Items { get; set; }
		public Command LoadItemsCommand { get; set; }

		public ItemsViewModel()
		{
			Title = "My Expenses";
			Items = new ObservableRangeCollection<ExpenseReport>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			MessagingCenter.Subscribe<ExpenseDetailPage, ExpenseReport>(this, "AddItem",  (obj, item) =>
			{
				var _item = item as ExpenseReport;
				Items.Add(_item);
				//await DataStore.AddItemAsync(_item);
			});
		}

		async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Items.Clear();
				//var items = await DataStore.GetItemsAsync(true);
				//Items.ReplaceRange(items);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessagingCenter.Send(new MessagingCenterAlert
				{
					Title = "Error",
					Message = "Unable to load items.",
					Cancel = "OK"
				}, "message");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}