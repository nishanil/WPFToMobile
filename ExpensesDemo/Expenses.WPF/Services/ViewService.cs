using System;
using System.Threading.Tasks;
using System.Windows;

namespace Expenses.WPF.Services
{
    public class ViewService : IViewService
    {
        private bool _isBusy;

        public event EventHandler<EventArgs<bool>> BusyChanged;

        public void ShowBusy(bool isBusy)
        {
            if (this._isBusy == isBusy) { return; }

            this._isBusy = isBusy;

            EventHandler<EventArgs<bool>> handler = this.BusyChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<bool>(this._isBusy));
            }
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message);
        }

        public Task<bool> ConfirmAsync(string title, string message)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            tcs.SetResult(MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK);

            return tcs.Task;
        }

        async Task IViewService.ExecuteBusyActionAsync(Func<Task> func)
        {
            this.ShowBusy(true);
            try
            {
                await func();
            }
            catch (Exception e)
            {
                this.ShowError(e.ToString());
            }
            finally
            {
                this.ShowBusy(false);
            }
        }

        async Task<T> IViewService.ExecuteBusyActionAsync<T>(Func<Task<T>> func)
        {
            this.ShowBusy(true);
            try
            {
                return await func();
            }
            catch (Exception e)
            {
                this.ShowError(e.ToString());
            }
            finally
            {
                this.ShowBusy(false);
            }

            return default(T);
        }
    }
}
