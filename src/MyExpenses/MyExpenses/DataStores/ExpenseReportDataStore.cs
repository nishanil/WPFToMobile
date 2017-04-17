using System;
using System.Collections.Generic;
using System.Text;
using MyExpenses.DataStores;
using MyExpenses.Models;

[assembly: Xamarin.Forms.Dependency(typeof(ExpenseReportDataStore))]
namespace MyExpenses.DataStores
{
    public class ExpenseReportDataStore : AzureDataStore<ExpenseReport>
    {
    }
}
