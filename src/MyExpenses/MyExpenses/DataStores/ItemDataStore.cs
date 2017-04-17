using MyExpenses.Models;
using MyExpenses.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(ItemDataStore))]
namespace MyExpenses.Stores
{
    
    public class ItemDataStore : AzureDataStore<Item>

    {
    }
}
