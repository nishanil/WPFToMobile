using Expenses.WPF.ExpensesService;
using Expenses.WPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.WPF.ViewModels
{
    public class ExpenseReportViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region Properties

        private int _expenseReportId = 0;
        public int ExpenseReportId
        {
            get
            { return _expenseReportId; }

            set
            {
                if (_expenseReportId == value)
                { return; }

                _expenseReportId = value;
                this.NotifyOfPropertyChange(() => this.ExpenseReportId);
            }
        }

        private int employeeId = 0;
        public int EmployeeId
        {
            get
            { return employeeId; }

            set
            {
                if (employeeId == value)
                { return; }

                employeeId = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.EmployeeId);
            }
        }

        private string employeeName = string.Empty;

        private decimal amount = 0M;
        public decimal Amount
        {
            get
            { return amount; }

            set
            {
                if (amount == value)
                { return; }

                amount = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Amount);
            }
        }

        private string approver = string.Empty;
        public string Approver
        {
            get
            { return approver; }

            set
            {
                if (approver == value)
                { return; }

                approver = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Approver);
            }
        }

        private int costCenter = 0;
        public int CostCenter
        {
            get
            { return costCenter; }

            set
            {
                if (costCenter == value)
                { return; }

                costCenter = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.CostCenter);
            }
        }

        private string notes = string.Empty;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                if (notes == value)
                { return; }

                notes = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Notes);
            }
        }

        private DateTime? dateSaved = null;
        public DateTime? DateSaved
        {
            get
            {
                return dateSaved;
            }

            set
            {
                if (dateSaved == value)
                { return; }

                dateSaved = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.DateSaved);
            }
        }

        private DateTime? dateSubmitted = null;
        public DateTime? DateSubmitted
        {
            get
            {
                return dateSubmitted;
            }

            set
            {
                if (dateSubmitted == value)
                { return; }

                dateSubmitted = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.DateSubmitted);
            }
        }

        private ExpenseReportStatus status = ExpenseReportStatus.Saved;
        public ExpenseReportStatus Status
        {
            get
            {
                return status;
            }

            set
            {
                if (status == value)
                { return; }

                status = value;
                this.Modified = true;

                this.NotifyOfPropertyChange(() => this.Status);
            }
        }

        private DateTime? dateResolved = null;
        public DateTime? DateResolved
        {
            get
            {
                return dateResolved;
            }

            set
            {
                if (dateResolved == value)
                { return; }

                dateResolved = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.DateResolved);
            }
        }

        private decimal owedToCreditCard = 0M;
        public decimal OwedToCreditCard
        {
            get
            {
                return owedToCreditCard;
            }

            set
            {
                if (owedToCreditCard == value)
                { return; }

                owedToCreditCard = value;
                this.NotifyOfPropertyChange(() => this.OwedToCreditCard);
            }
        }

        private decimal owedToEmployee = 0M;
        public decimal OwedToEmployee
        {
            get
            {
                return owedToEmployee;
            }

            set
            {
                if (owedToEmployee == value)
                { return; }

                owedToEmployee = value;
                this.NotifyOfPropertyChange(() => this.OwedToEmployee);
            }
        }

        private string backgroundColor;
        public string BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
                this.NotifyOfPropertyChange(() => this.BackgroundColor);
            }
        }

        private string foregroundColor;
        public string ForegroundColor
        {
            get
            {
                return foregroundColor;
            }

            set
            {
                foregroundColor = value;
                this.NotifyOfPropertyChange(() => this.ForegroundColor);
            }
        }

        private bool _modified = false;
        public bool Modified
        {
            get
            {
                return _modified;
            }

            set
            {
                _modified = value;
                var errorsChangedEvent = this.ErrorsChanged;
                if (errorsChangedEvent != null)
                {
                    errorsChangedEvent.Invoke(this, new DataErrorsChangedEventArgs(null));
                }

                this.NotifyOfPropertyChange(() => this.Modified);
            }
        }

        public DateTime DisplayDate
        {
            get { return this._displayDate; }
            set
            {
                this._displayDate = value;
                this.NotifyOfPropertyChange(() => this.DisplayDate);
            }
        }
        private DateTime _displayDate;

        #endregion "Properties"

        private readonly INavigationService _navigationService;
        private readonly IRepositoryService _repositoryService;
        private readonly IViewService _viewService;
        private readonly ICurrentIdentityService _currentIdentityService;

        public ICommand SaveReportCommand { get; private set; }
        public ICommand DeleteReportCommand { get; private set; }
        public ICommand SubmitReportCommand { get; private set; }
        public ICommand ApproveReportCommand { get; private set; }

        public ExpenseReportViewModel(IServiceFactory serviceFactory)
        {
            this._currentIdentityService = serviceFactory.CurrentIdentityService;
            this._navigationService = serviceFactory.NavigationService;
            this._repositoryService = serviceFactory.RepositoryService;
            this._viewService = serviceFactory.ViewService;

            employeeId = this._currentIdentityService.EmployeeId;
            employeeName = this._currentIdentityService.Name;
            approver = this._currentIdentityService.Manager;
            costCenter = 50992;

            this.SaveReportCommand = new RelayCommand(
                async () =>
                {
                    await this.SaveAsync();
                });

            this.DeleteReportCommand = new RelayCommand(
                async () =>
                {
                    await this.DeleteAsync();
                });

            this.SubmitReportCommand = new RelayCommand(
                async () =>
                {
                    await this.SubmitAsync();
                });

            this.ApproveReportCommand = new RelayCommand(
                async () =>
                {
                    await this.ApproveAsync();
                });
        }

        internal ExpenseReportViewModel(ExpenseReport expenseReport, IServiceFactory serviceFactory)
            : this(serviceFactory)
        {
            this.Load(expenseReport);
        }


        public async Task LoadAsync(int expenseReportId)
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // extract ExpenseReport from WCF service
                    var expenseReport = await this._repositoryService.GetExpenseReportAsync(expenseReportId);
                    this.Load(expenseReport);
                });
        }

        private void Load(ExpenseReport expenseReport)
        {
            this.Amount = expenseReport.Amount;
            this.Approver = expenseReport.Approver;
            this.CostCenter = expenseReport.CostCenter;
            this.DateResolved = expenseReport.DateResolved;
            this.DateSubmitted = expenseReport.DateSubmitted;
            this.EmployeeId = expenseReport.EmployeeId;
            this.ExpenseReportId = expenseReport.ExpenseReportId;
            this.Notes = expenseReport.Notes;
            this.Status = expenseReport.Status;

            switch (this.Status)
            {
                case ExpenseReportStatus.Saved: this.DisplayDate = this.DateSaved.GetValueOrDefault(); break;
                case ExpenseReportStatus.Approved: this.DisplayDate = this.DateResolved.GetValueOrDefault(); break;
                case ExpenseReportStatus.Submitted: this.DisplayDate = this.DateSubmitted.GetValueOrDefault(); break;
            }
        }

        public async Task SaveAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    ExpenseReport expenseReport = new ExpenseReport()
                    {
                        Amount = this.Amount,
                        Approver = this.Approver,
                        CostCenter = this.CostCenter,
                        DateResolved = this.DateResolved.GetValueOrDefault(),
                        DateSubmitted = this.DateSubmitted.GetValueOrDefault(),
                        EmployeeId = this.EmployeeId,
                        ExpenseReportId = this.ExpenseReportId,
                        Notes = this.Notes,
                        Status = this.Status,
                    };

                    // save expense report to repository
                    if (this.ExpenseReportId == 0)
                    {
                        this.ExpenseReportId = await this._repositoryService.CreateNewExpenseReportAsync(expenseReport);
                    }
                    else
                    {
                        this.ExpenseReportId = await this._repositoryService.UpdateExpenseReportAsync(expenseReport);
                    }
                    
                    await this.LoadAsync(this.ExpenseReportId);
                });
        }

        public async Task DeleteAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // Delete report
                    await this._repositoryService.DeleteExpenseReportAsync(this.ExpenseReportId);
                });

            this._navigationService.ShowSavedExpenseReports();
        }

        public async Task SubmitAsync()
        {
            this.Status = ExpenseReportStatus.Submitted;
            this.DateSubmitted = DateTime.Now;

            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    await this.SaveAsync();
                });

            this._navigationService.ShowSubmittedExpenseReports();
        }

        public async Task ApproveAsync()
        {
            this.Status = ExpenseReportStatus.Approved;
            this.DateResolved = DateTime.Now;

            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    await this.SaveAsync();
                });

            this._navigationService.ShowApprovedExpenseReports();
        }

        #region INotifyDataErrorInfo implementation
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();

            if (propertyName == null || Utilities.GetPropertyName(() => this.Notes) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Notes))
                {
                    errors.Add("Notes are required");
                }
                if (!Utilities.ValidateStringLength(this.Notes, 250))
                {
                    errors.Add("Notes cannot be greater than 250 characters");
                }
            }

            if (propertyName == null || Utilities.GetPropertyName(() => this.Approver) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Approver))
                {
                    errors.Add("Approver is required");
                }
                if (!Utilities.ValidateStringLength(this.Approver, 25))
                {
                    errors.Add("Approver cannot be greater than 25 characters");
                }
            }

            return errors;
        }

        public bool HasErrors
        {
            get
            {
                return this.GetErrors(null).Cast<string>().Count() > 0;
            }
        }

        #endregion INotifyDataErrorInfo implementation
    }
}
