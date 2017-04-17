using MyExpenses.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;

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

        public async Task ShowError(string message)
        {
            // MessageBox.Show(message);
            await MyExpenses.App.Current.MainPage.DisplayAlert("MyExpenses", message, "OK");
            
        }

        public async Task<bool> ConfirmAsync(string title, string message)
        {
            //TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            //tcs.SetResult(MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK);

            //return tcs.Task;

            return await MyExpenses.App.Current.MainPage.DisplayAlert("MyExpenses", message, "OK", "Cancel");


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
                await this.ShowError(e.ToString());
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
                await this.ShowError(e.ToString());
            }
            finally
            {
                this.ShowBusy(false);
            }

            return default(T);
        }
    }
}
