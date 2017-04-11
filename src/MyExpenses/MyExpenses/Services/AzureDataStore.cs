using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using MyExpenses.Helpers;
using MyExpenses.Models;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;

namespace MyExpenses.Services
{
	public class AzureDataStore : IDataStore<Item>
	{
        public bool UseAuthentication => false;
        public MobileServiceAuthenticationProvider AuthProvider => MobileServiceAuthenticationProvider.Facebook;

        bool isInitialized;
		IMobileServiceSyncTable<Item> itemsTable;

		public MobileServiceClient MobileService { get; set; }

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			await InitializeAsync();
			if (forceRefresh)
				await PullLatestAsync();

			return await itemsTable.ToEnumerableAsync();
		}

		public async Task<Item> GetItemAsync(string id)
		{
			await InitializeAsync();
			await PullLatestAsync();
			var items = await itemsTable.Where(s => s.Id == id).ToListAsync();

			if (items == null || items.Count == 0)
				return null;

			return items[0];
		}

		public async Task<bool> AddItemAsync(Item item)
		{
			await InitializeAsync();
			await PullLatestAsync();
			await itemsTable.InsertAsync(item);
			await SyncAsync();

			return true;
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			await InitializeAsync();
			await itemsTable.UpdateAsync(item);
			await SyncAsync();

			return true;
		}

		public async Task<bool> DeleteItemAsync(Item item)
		{
			await InitializeAsync();
			await PullLatestAsync();
			await itemsTable.DeleteAsync(item);
			await SyncAsync();

			return true;
		}

		public async Task InitializeAsync()
		{
			if (isInitialized)
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
				MobileService.CurrentUser = new MobileServiceUser(Settings.UserId);
				MobileService.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
			}

			var store = new MobileServiceSQLiteStore("app.db");
			store.DefineTable<Item>();
			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
			itemsTable = MobileService.GetSyncTable<Item>();

			isInitialized = true;
		}

		public async Task<bool> PullLatestAsync()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				Debug.WriteLine("Unable to pull items, we are offline");
				return false;
			}
			try
			{
				await itemsTable.PullAsync($"all{typeof(Item).Name}", itemsTable.CreateQuery());
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to pull items, that is alright as we have offline capabilities: " + ex);
				return false;
			}
			return true;
		}


		public async Task<bool> SyncAsync()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				Debug.WriteLine("Unable to sync items, we are offline");
				return false;
			}
			try
			{
				await MobileService.SyncContext.PushAsync();
				if (!(await PullLatestAsync().ConfigureAwait(false)))
					return false;
			}
			catch (MobileServicePushFailedException exc)
			{
				if (exc.PushResult == null)
				{
					Debug.WriteLine("Unable to sync, that is alright as we have offline capabilities: " + exc);
					return false;
				}
				foreach (var error in exc.PushResult.Errors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync();
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to sync items, that is alright as we have offline capabilities: " + ex);
				return false;
			}

			return true;
		}
	}
}