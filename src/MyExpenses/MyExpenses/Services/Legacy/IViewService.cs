using MyExpenses.Helpers;
using System;
using System.Threading.Tasks;

namespace Expenses.WPF.Services
{
    public interface IViewService
    {
        event EventHandler<EventArgs<bool>> BusyChanged;

        void ShowBusy(bool isBusy);

        Task ShowError(string message);

        Task<bool> ConfirmAsync(string title, string message);

        Task ExecuteBusyActionAsync(Func<Task> func);

        Task<T> ExecuteBusyActionAsync<T>(Func<Task<T>> func);
    }
}
