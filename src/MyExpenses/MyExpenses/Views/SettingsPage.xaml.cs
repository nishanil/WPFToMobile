using MyExpenses.DataStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyExpenses.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();

            SyncDataCommand = new Command(async () =>
            {
                await DependencyService.Get<IDataManager>().SyncAll();
                await DisplayAlert("Expenses", "Sync Complete", "Ok");
            });

            BindingContext = this;
        }

        public ICommand SyncDataCommand { get; set; }

 
    }
}
