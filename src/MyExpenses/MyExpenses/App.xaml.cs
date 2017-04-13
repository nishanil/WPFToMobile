using System.Collections.Generic;

using MyExpenses.Helpers;
using MyExpenses.Services;
using MyExpenses.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyExpenses
{
	public partial class App : Application
	{
        //MUST use HTTPS, neglecting to do so will result in runtime errors on iOS
		public static bool AzureNeedsSetup => AzureMobileAppUrl == "https://CONFIGURE-THIS-URL.azurewebsites.net";
		public static string AzureMobileAppUrl = "https://myexpenses-si.azurewebsites.net";
        public static IDictionary<string, string> LoginParameters => null;

 
        public App()
		{
			InitializeComponent();

			if (AzureNeedsSetup)
				DependencyService.Register<MockDataStore>();
			else
				DependencyService.Register<AzureDataStore>();

			SetMainPage();
		}

		public static void SetMainPage()
		{
            if (!AzureNeedsSetup && !Settings.IsLoggedIn)
            {
                Current.MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = (Color)Current.Resources["Primary"],
                    BarTextColor = Color.White
                };
            }
            else
            {
                GoToMainPage();
            }
		}

        public static void GoToMainPage()
        {
            Current.MainPage = new TabbedPage
			{
				Children =
				{
                    new NavigationPage(new ExpensesPage())
                    {
                        Title = "Expenses",
                        Icon = Device.OnPlatform("CreditCard.png",null,null)
                    },
                    new NavigationPage(new ReportsPage())
					{
						Title = "Reports",
						Icon = Device.OnPlatform("Chart.png",null,null)
					},
					new NavigationPage(new AboutPage())
					{
						Title = "Settings",
						Icon = Device.OnPlatform("Settings.png",null,null)
					},
                    new NavigationPage(new AboutPage())
                    {
                        Title = "Profile",
                        Icon = Device.OnPlatform("Person.png",null,null)
                    },
                }
			};
        }
	}
}
