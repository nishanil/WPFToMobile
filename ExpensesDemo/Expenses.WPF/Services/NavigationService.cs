using Expenses.WPF.ViewModels;
using System;

namespace Expenses.WPF.Services
{
    public class NavigationService : INavigationService
    {
        public event EventHandler<EventArgs<ChargeViewModel>> ShowChargeRequested;
        public event EventHandler ShowChargesRequested;
        public event EventHandler<EventArgs<ExpenseReportViewModel>> ShowExpenseReportRequested;
        public event EventHandler ShowSubmittedExpenseReportsRequested;
        public event EventHandler ShowSavedExpenseReportsRequested;
        public event EventHandler ShowApprovedExpenseReportsRequested;

        public void ShowCharge(ChargeViewModel chargeViewModel)
        {
            EventHandler<EventArgs<ChargeViewModel>> handler = this.ShowChargeRequested;
            if (handler != null)
            {
                handler(this, new EventArgs<ChargeViewModel>(chargeViewModel));
            }
        }

        public void ShowCharges()
        {
            EventHandler handler = this.ShowChargesRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel)
        {
            EventHandler<EventArgs<ExpenseReportViewModel>> handler = this.ShowExpenseReportRequested;
            if (handler != null)
            {
                handler(this, new EventArgs<ExpenseReportViewModel>(expenseReportViewModel));
            }
        }

        public void ShowSubmittedExpenseReports()
        {
            EventHandler handler = this.ShowSubmittedExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowSavedExpenseReports()
        {
            EventHandler handler = this.ShowSavedExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowApprovedExpenseReports()
        {
            EventHandler handler = this.ShowApprovedExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
