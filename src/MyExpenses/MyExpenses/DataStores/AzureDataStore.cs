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
using MyExpenses.Services;

namespace MyExpenses.DataStores
{
	public class AzureDataStore<T> : IDataStore<T> where T : BaseDataObject, new()
	{
        bool isInitialized;
		IMobileServiceSyncTable<T> itemsTable;

		public MobileServiceClient MobileService { get; private set; }

		public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh)
				await PullLatestAsync();

			return await itemsTable.ToEnumerableAsync();
		}

		public async Task<T> GetItemAsync(string id)
		{
			await PullLatestAsync();
			var items = await itemsTable.Where(s => s.Id == id).ToListAsync();

			if (items == null || items.Count == 0)
				return null;

			return items[0];
		}

		public async Task<bool> AddItemAsync(T item)
		{
			await PullLatestAsync();
			await itemsTable.InsertAsync(item);
			await SyncAsync();

			return true;
		}

		public async Task<bool> UpdateItemAsync(T item)
		{
			await itemsTable.UpdateAsync(item);
			await SyncAsync();

			return true;
		}

		public async Task<bool> DeleteItemAsync(T item)
		{

			await PullLatestAsync();
			await itemsTable.DeleteAsync(item);
			await SyncAsync();

			return true;
		}

        public Task<bool> InitializeAsync(MobileServiceClient client,
            MobileServiceSQLiteStore store)
        {
            MobileService = client;
            store.DefineTable<T>();
            itemsTable = MobileService.GetSyncTable<T>();
            isInitialized = true;

            return Task.FromResult<bool>(isInitialized);

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
                //TODO: Query on logged in Employee Id
				await itemsTable.PullAsync($"all{typeof(T).Name}", itemsTable.CreateQuery());
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