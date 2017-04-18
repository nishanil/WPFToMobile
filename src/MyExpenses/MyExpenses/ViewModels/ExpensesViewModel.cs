using Expenses.WPF.Services;
using MyExpenses.Helpers;
using MyExpenses.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;

namespace MyExpenses.ViewModels
{
    public class ExpensesViewModel : BaseViewModel
    {
        public IRepositoryService Repo => DependencyService.Get<IRepositoryService>();

        public ObservableRangeCollection<Charge> Items { get; }
        public ObservableRangeCollection<Grouping<string, Charge>> ItemsGrouped { get; }

        public string TotalAmount { get { return Items.Sum(x => x.TransactionAmount).ToString("c"); } }

        public ExpensesViewModel()
        {
            Title = "Charges";

            Items = new ObservableRangeCollection<Charge>();

            ItemsGrouped = new ObservableRangeCollection<Grouping<string, Charge>>();

            RefreshDataCommand = new Command(
                async () => await RefreshData());

        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                ItemsGrouped.Clear();
                var employee = await Repo.GetEmployeeAsync(App.DefaultEmployeeAlias);
                //TODO: Temp Hack. Fix this when login page is ready
                App.EmployeeId = employee.Id;
                DependencyService.Get<CurrentIdentityService>().SetNewIdentity(employee.Alias, employee.Id, employee.Manager, employee.Name, false);

                var items = await Repo.GetOutstandingChargesForEmployeeAsync(employee.Id);
                Items.ReplaceRange(items);

                var sorted = from item in Items
                             orderby item.ExpenseDate descending
                             group item by item.ExpenseDate.ToString("MMMM") into itemGroup
                             select new Grouping<string, Charge>(itemGroup.Key, itemGroup);
                ItemsGrouped.ReplaceRange(sorted);


                OnPropertyChanged("TotalAmount");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }


        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }
    }
}
