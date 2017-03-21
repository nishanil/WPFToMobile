using Expenses.WPF.ExpensesService;
using Expenses.WPF.Services;
using System.Linq;
using System.Windows.Input;

namespace Expenses.WPF.ViewModels
{
    class EditExpenseReportViewModel : ViewModelBase
    {
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                if (this._canSave == value) { return; }

                this._canSave = value;
                this.NotifyOfPropertyChange(() => this.CanSave);
            }
        }
        private bool _canSave = true;

        public bool CanSubmit
        {
            get { return this._canSubmit; }
            set
            {
                if (this._canSubmit == value) { return; }

                _canSubmit = value;
                this.NotifyOfPropertyChange(() => this.CanSubmit);
            }
        }
        private bool _canSubmit = true;

        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                if (this._canDelete == value) { return; }

                this._canDelete = value;
                this.NotifyOfPropertyChange(() => this.CanDelete);
            }
        }
        private bool _canDelete = true;

        public bool CanApprove
        {
            get { return _canApprove; }
            set
            {
                if (this._canApprove == value) { return; }

                this._canApprove = value;
                this.NotifyOfPropertyChange(() => this.CanApprove);
            }
        }
        private bool _canApprove = true;

        public bool CanModifyCharges
        {
            get { return _canModifyCharges; }
            set
            {
                if (this._canModifyCharges == value) { return; }

                this._canModifyCharges = value;
                this.NotifyOfPropertyChange(() => this.CanModifyCharges);
            }
        }
        private bool _canModifyCharges;

        public bool IsNewReport
        {
            get { return this._isNewReport; }
            set
            {
                if (this._isNewReport == value) { return; }

                this._isNewReport = value;
                this.NotifyOfPropertyChange(() => this.IsNewReport);
            }
        }
        private bool _isNewReport = true;

        public ExpenseReportViewModel ExpenseReport
        {
            get { return _expenseReportViewModel; }
            set
            {
                if (this._expenseReportViewModel != null)
                {
                    this._expenseReportViewModel.ErrorsChanged -= OnExpenseReportViewModel_ErrorsChanged;
                }
                _expenseReportViewModel = value;

                if (this._expenseReportViewModel != null)
                {
                    this._expenseReportViewModel.ErrorsChanged += OnExpenseReportViewModel_ErrorsChanged;
                }
                this.UpdateBindableProperties();

                this.UpdateChargeSums();
            }
        }

        void OnExpenseReportViewModel_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            ((RelayCommand)this.SaveReportCommand).OnCanExecuteChanged();
        }

        private ExpenseReportViewModel _expenseReportViewModel;

        public AddChargesViewModel AddCharges
        {
            get { return _addChargesViewModel; }
            set 
            { 
                _addChargesViewModel = value;
                this.UpdateChargeSums();
            }
        }
        private AddChargesViewModel _addChargesViewModel;

        public ExpenseReportChargesViewModel AssociatedCharges
        {
            get { return _expenseReportChargesView; }
            set { _expenseReportChargesView = value; }
        }
        private ExpenseReportChargesViewModel _expenseReportChargesView;

        public ICommand AddChargeToReportCommand { get; private set; }
        public ICommand RemoveChargeFromReportCommand { get; private set; }
        public ICommand SaveReportCommand { get; private set; }

        private readonly IViewService ViewService;

        public EditExpenseReportViewModel(IServiceFactory serviceFactory)
        {
            this.ViewService = serviceFactory.ViewService;

            this.AddChargeToReportCommand =
                new RelayCommand<ChargeViewModel>(
                    (charge) =>
                    {
                        this.AddChargeToReport(charge);
                    });

            this.RemoveChargeFromReportCommand =
                new RelayCommand<ChargeViewModel>(
                    (charge) =>
                    {
                        this.RemoveChargeFromReport(charge);
                    });

            this.SaveReportCommand =
                new RelayCommand(
                    async () =>
                    {
                        UpdateChargeSums();

                        await this.ViewService.ExecuteBusyActionAsync(
                            async () =>
                            {
                                await this._expenseReportViewModel.SaveAsync();
                                await this._addChargesViewModel.SaveChargesAsync();
                                await this._expenseReportChargesView.SaveChargesAsync();
                            });
                        this.UpdateBindableProperties();
                    },
                    () =>
                    {
                        return !this.ExpenseReport.HasErrors;
                    });
        }

        private void UpdateChargeSums()
        {
            if (this._expenseReportViewModel != null && this._expenseReportChargesView != null)
            {
                this._expenseReportViewModel.Amount = this._expenseReportChargesView.Charges.Sum(item => item.BilledAmount);
                this._expenseReportViewModel.OwedToCreditCard = this._expenseReportChargesView.Charges.Sum(item => item.TransactionAmount);
                this._expenseReportViewModel.OwedToEmployee = this._expenseReportViewModel.Amount - this._expenseReportViewModel.OwedToCreditCard;
            }
        }

        private void UpdateBindableProperties()
        {
            this.IsNewReport = (this._expenseReportViewModel.ExpenseReportId == 0);
            this.CanDelete = (this._expenseReportViewModel.Status == ExpenseReportStatus.Saved) && !this.IsNewReport;
            this.CanSubmit = (this._expenseReportViewModel.Status == ExpenseReportStatus.Saved) && !this.IsNewReport;
            this.CanSave = (this._expenseReportViewModel.Status == ExpenseReportStatus.Saved) || (this._expenseReportViewModel.ExpenseReportId == 0);
            this.CanApprove = this._expenseReportViewModel.Status == ExpenseReportStatus.Submitted;
            this.CanModifyCharges = this.CanSave && !this.IsNewReport;
        }

        private void AddChargeToReport(ChargeViewModel chargeViewModel)
        {
            this._addChargesViewModel.Charges.Remove(chargeViewModel);

            chargeViewModel.ExpenseReportId = this.ExpenseReport.ExpenseReportId;
            this._expenseReportChargesView.Charges.Add(chargeViewModel);

            this.UpdateChargeSums();
        }

        private void RemoveChargeFromReport(ChargeViewModel chargeViewModel)
        {
            chargeViewModel.ExpenseReportId = null;
            this._expenseReportChargesView.Charges.Remove(chargeViewModel);
            this._addChargesViewModel.Charges.Add(chargeViewModel);

            this.UpdateChargeSums();
        }
    }
}
