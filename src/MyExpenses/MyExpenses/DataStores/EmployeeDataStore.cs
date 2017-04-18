using MyExpenses.DataStores;
using MyExpenses.Models;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(EmployeeDataStore))]

namespace MyExpenses.DataStores
{
    public class EmployeeDataStore : AzureDataStore<Employee>
    {
    }
}
