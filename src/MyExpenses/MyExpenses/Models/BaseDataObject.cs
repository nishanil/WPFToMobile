using MyExpenses.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace MyExpenses.Models
{
	public class BaseDataObject : ObservableObject
	{
		public BaseDataObject()
		{
			Id = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Id for item
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Azure created at time stamp
		/// </summary>
		[JsonProperty(PropertyName = "createdAt")]
		public DateTimeOffset CreatedAt { get; set; }

		/// <summary>
		/// Azure UpdateAt timestamp for online/offline sync
		/// </summary>
		[UpdatedAt]
		public DateTimeOffset UpdatedAt { get; set; }

		/// <summary>
		/// Azure version for online/offline sync
		/// </summary>
		[Version]
		public string AzureVersion { get; set; }
	}
}