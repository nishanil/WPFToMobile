using MyExpenses.Models;
using MyExpenses.DataStores;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(ItemDataStore))]
namespace MyExpenses.DataStores
{
    
    public class ItemDataStore : AzureDataStore<Item>

    {
    }
}
