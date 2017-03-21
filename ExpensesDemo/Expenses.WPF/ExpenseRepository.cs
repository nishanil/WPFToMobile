using Expenses.WPF.ExpensesService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.WPF
{
    public class ExpenseRepository
    {
        private ExpenseServiceClient CreateExpenseServiceClient()
        {
            return new ExpenseServiceClient();
        }

        private bool VerifyResult<T>(AsyncCompletedEventArgs args, TaskCompletionSource<T> tcs)
        {
            if (args.Cancelled)
            {
                tcs.TrySetCanceled();
            }
            else if (args.Error != null)
            {
                tcs.TrySetException(args.Error);
            }
            else
            {
                return true;
            }

            return false;
        }

        public Task<Employee> GetEmployeeAsync(string employeeAlias)
        {
            var client = this.CreateExpenseServiceClient();

            return client.GetEmployeeAsync(employeeAlias);
        }
    }
}
