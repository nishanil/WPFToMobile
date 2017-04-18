using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using MyExpenses.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyExpenses.Helpers;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Xamarin.Forms;
using MyExpenses.DataStores;
using MyExpenses.Models;
using System.Linq;

[assembly: Dependency(typeof(AzureDataManager))]
namespace MyExpenses.DataStores
{
    public interface IDataManager
    {
        Task Init();
        bool UseAuthentication { get; }
        MobileServiceAuthenticationProvider AuthProvider { get;  }
        MobileServiceClient MobileService { get; }

        Task<bool> SyncAll();

    }
    public class AzureDataManager : IDataManager
    {
        public MobileServiceSQLiteStore SQLiteStore { get; private set; } 
        public MobileServiceClient MobileService { get; private set; }
        public bool UseAuthentication => false;
        public MobileServiceAuthenticationProvider AuthProvider => MobileServiceAuthenticationProvider.Facebook;

        public bool IsInitialized { get; private set; } = false;
        public async Task Init()
        {
            if (IsInitialized)
                return;

            AuthenticationHandler handler = null;

            if (UseAuthentication)
                handler = new AuthenticationHandler();

            MobileService = new MobileServiceClient(App.AzureMobileAppUrl, handler)
            {
                SerializerSettings = new MobileServiceJsonSerializerSettings
                {
                    CamelCasePropertyNames = true
                }
            };

            if (UseAuthentication && !string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                MobileService.CurrentUser = new MobileServiceUser(Settings.UserId)
                {
                    MobileServiceAuthenticationToken = Settings.AuthToken
                };
            }

            SQLiteStore = new MobileServiceSQLiteStore("myexpensesstore.db");

            await InitDataStores();

            await MobileService.SyncContext.InitializeAsync(SQLiteStore, new MobileServiceSyncHandler());

            IsInitialized = true;
        }

        public async Task<bool> SyncAll()
        {
            var taskList = new List<Task<bool>>();
            taskList.Add(DependencyService.Get<EmployeeDataStore>().SyncAsync());
            taskList.Add(DependencyService.Get<ExpenseReportDataStore>().SyncAsync());
            taskList.Add(DependencyService.Get<ChargeDataStore>().SyncAsync());


            var successes = await Task.WhenAll(taskList).ConfigureAwait(false);
            return successes.Any(x => !x);

        }

        private async Task InitDataStores()
        {
            await DependencyService.Get<EmployeeDataStore>().InitializeAsync(MobileService, SQLiteStore);
            await DependencyService.Get<ExpenseReportDataStore>().InitializeAsync(MobileService, SQLiteStore);
            await DependencyService.Get<ChargeDataStore>().InitializeAsync(MobileService, SQLiteStore);

        }
    }
}
