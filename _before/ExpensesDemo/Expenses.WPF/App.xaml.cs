using Expenses.WPF.ExpensesService;
using Expenses.WPF.Services;
using Expenses.WPF.ViewModels;
using Expenses.WPF.Views;
using Microsoft.Practices.Unity;
using System;
using System.Windows;

namespace Expenses.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string DefaultEmployeeAlias = "kimakers";

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            base.OnStartup(eventArgs);

            // create Unity container
            var container = new UnityContainer();

            // instantiate services
            var navigationService = new NavigationService();
            var viewService = new ViewService();
            var repositoryService = new RepositoryService(viewService);
            var currentIdentityService = new CurrentIdentityService();

            // register service instances with Unity container
            container
                .RegisterInstance<INavigationService>(navigationService)
                .RegisterInstance<IViewService>(viewService)
                .RegisterInstance<IRepositoryService>(repositoryService)
                .RegisterInstance<ICurrentIdentityService>(currentIdentityService);

            MainWindow mainWindow = new MainWindow();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(new ServiceFactoryImpl(container));

            mainWindow.DataContext = mainWindowViewModel;

            currentIdentityService.IdentityChanged += (_, __) => { mainWindowViewModel.OnCurrentIdentityChanged(); };

            // Hook up the mainWindowViewModel busy state with viewService busy state.
            viewService.BusyChanged += ((_, e) => 
            {
                mainWindowViewModel.IsBusy = e.Data;
            });

            // Initiate an asynchronous operation to fetch initial employee data.
            ((IViewService)viewService)
                .ExecuteBusyActionAsync<Employee>(() => 
                    {
                        return repositoryService.GetEmployeeAsync(DefaultEmployeeAlias);
                    })
                .ContinueWith((e) =>
                    {
                        currentIdentityService.SetNewIdentity(e.Result.Alias, e.Result.EmployeeId, e.Result.Manager, e.Result.Name, false);

                        mainWindowViewModel.ShowChargesAsync();
                    });

            // Wire up navigation events to execute actions on appropriate view-models.
            navigationService.ShowChargeRequested += (_, ea) => { mainWindowViewModel.ShowCharge(ea.Data); };
            navigationService.ShowChargesRequested += (_, __) => { mainWindowViewModel.ShowChargesAsync(); };
            navigationService.ShowExpenseReportRequested += async (_, ea) => { await mainWindowViewModel.ShowExpenseReportAsync(ea.Data); };
            navigationService.ShowApprovedExpenseReportsRequested += async (_, __) => { await mainWindowViewModel.ShowApprovedReportsAsync(); };
            navigationService.ShowSubmittedExpenseReportsRequested += async (_, __) => { await mainWindowViewModel.ShowSubmittedReportsAsync(); };
            navigationService.ShowSavedExpenseReportsRequested += async (_, __) => { await mainWindowViewModel.ShowSavedReportsAsync(); };

            // Connect main window' view-model's close event to main window view action.
            EventHandler handler = null;
            handler = delegate
            {
                mainWindowViewModel.RequestClose -= handler;
                mainWindow.Close();
            };
            mainWindowViewModel.RequestClose += handler;

            // Show up the main window.
            mainWindow.Show();
        }
    }
}
