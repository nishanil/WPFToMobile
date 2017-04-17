using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyExpenses.DataStores
{
	public interface IDataStore<T>
	{
		Task<bool> AddItemAsync(T item);
		Task<bool> UpdateItemAsync(T item);
		Task<bool> DeleteItemAsync(T item);
		Task<T> GetItemAsync(string id);
		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

		Task<bool> InitializeAsync(MobileServiceClient client, MobileServiceSQLiteStore store);
		Task<bool> PullLatestAsync();
		Task<bool> SyncAsync();
	}
}
