using MyExpenses.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MyExpenses.DataStores;

[assembly: Xamarin.Forms.Dependency(typeof(ChargeDataStore))]

namespace MyExpenses.DataStores
{
    public class ChargeDataStore : AzureDataStore<Charge>
    {
    }
}
