using Expenses.WPF.ExpensesService;
using Expenses.WPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Expenses.WPF.ViewModels
{
    public class ChargeViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly IRepositoryService _repositoryService;
        private readonly INavigationService _navigationService;
        private readonly IViewService _viewService;

        #region Properties

        private int _chargeId = 0;
        public int ChargeId
        {
            get
            { return _chargeId; }

            set
            {
                if (_chargeId == value) { return; }

                _chargeId = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.ChargeId);
            }
        }

        private int _employeeId = 0;
        public int EmployeeId
        {
            get
            { return _employeeId; }

            set
            {
                if (_employeeId == value) { return; }

                _employeeId = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.EmployeeId);
            }
        }

        private int? _expenseReportId;
        public int? ExpenseReportId
        {
            get
            { return _expenseReportId; }

            set
            {
                if (_expenseReportId == value) { return; }

                _expenseReportId = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.ExpenseReportId);
            }
        }

        private DateTime _expenseDate = DateTime.Today;
        public DateTime ExpenseDate
        {
            get
            {
                return _expenseDate;
            }

            set
            {
                if (_expenseDate == value) { return; }

                _expenseDate = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.ExpenseDate);
            }
        }

        private string _merchant = string.Empty;
        public string Merchant
        {
            get
            { return _merchant; }

            set
            {
                if (_merchant == value) { return; }

                _merchant = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Merchant);
            }
        }

        private string _location = string.Empty;
        public string Location
        {
            get
            { return _location; }

            set
            {
                if (_location == value) { return; }

                _location = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Location);
            }
        }

        private decimal _billedAmount = 0M;
        public decimal BilledAmount
        {
            get
            { return _billedAmount; }

            set
            {
                if (_billedAmount == value) { return; }

                _billedAmount = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.BilledAmount);
            }
        }

        private decimal _transactionAmount = 0M;
        public decimal TransactionAmount
        {
            get
            { return _transactionAmount; }

            set
            {
                if (_transactionAmount == value) { return; }

                _transactionAmount = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.TransactionAmount);
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get
            { return _description; }

            set
            {
                if (_description == value) { return; }

                _description = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get
            { return _notes; }

            set
            {
                if (_notes == value) { return; }

                _notes = value;
                this.Modified = true;
                this.NotifyOfPropertyChange(() => this.Notes);
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
                var errorsChangedEvent = this.ErrorsChanged;
                if (errorsChangedEvent != null)
                {
                    errorsChangedEvent.Invoke(this, new DataErrorsChangedEventArgs(null));
                }

                this.SaveChargeCommand.OnCanExecuteChanged();

                if (_modified == value) { return; }

                _modified = value;
                this.NotifyOfPropertyChange(() => this.Modified);
            }
        }

        #endregion "Properties"

        #region "Commands"

        private RelayCommand _saveChargeCommand;
        public RelayCommand SaveChargeCommand
        {
            get
            {
                if (this._saveChargeCommand == null)
                {
                    this._saveChargeCommand = new RelayCommand(
                        async () =>
                        {
                            // save a charge 
                            var charge = new Charge()
                            {
                                BilledAmount = this.BilledAmount,
                                ChargeId = this.ChargeId,
                                Description = this.Description,
                                EmployeeId = this.EmployeeId,
                                ExpenseDate = this.ExpenseDate,
                                ExpenseReportId = this.ExpenseReportId,
                                Location = this.Location,
                                Merchant = this.Merchant,
                                Notes = this.Notes,
                                TransactionAmount = this.TransactionAmount,
                            };
                            if (this.ChargeId > 0)
                            {
                                this.ChargeId = await this._repositoryService.UpdateChargeAsync(charge);
                            }
                            else 
                            {
                                this.ChargeId = await this._repositoryService.CreateNewChargeAsync(charge);
                            }

                            // navigate to previous page
                            this._navigationService.ShowCharges();
                        },
                        () =>
                        {
                            return this.Modified;
                        }
                        );
                }
                return this._saveChargeCommand;
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (this._cancelCommand == null)
                {
                    this._cancelCommand = new RelayCommand(
                        () =>
                        {
                            // navigate to previous page
                            this._navigationService.ShowCharges();
                        });
                }
                return this._cancelCommand;
            }
        }

        #endregion "Commands"

        public ChargeViewModel(Charge charge, IServiceFactory serviceFactory)
        {
            this._repositoryService = serviceFactory.RepositoryService;
            this._navigationService = serviceFactory.NavigationService;
            this._viewService = serviceFactory.ViewService;

            this._billedAmount = charge.BilledAmount;
            this._chargeId = charge.ChargeId;
            this._description = charge.Description;
            this._employeeId = charge.EmployeeId;
            this._expenseDate = charge.ExpenseDate;
            this._expenseReportId = charge.ExpenseReportId;
            this._location = charge.Location;
            this._merchant = charge.Merchant;
            this._notes = charge.Notes;
            this._transactionAmount = charge.TransactionAmount;

            this._modified = false;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();

            if (propertyName == null || Utilities.GetPropertyName(() => this.Description) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Description))
                {
                    errors.Add("Description is required");
                }
                if (!Utilities.ValidateStringLength(this.Description, 32))
                {
                    errors.Add("Description cannot be greater than 32 characters");
                }
            }

            if (propertyName == null || Utilities.GetPropertyName(() => this.Location) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Location))
                {
                    errors.Add("Location is required");
                }
                if (!Utilities.ValidateStringLength(this.Location, 32))
                {
                    errors.Add("Location cannot be greater than 32 characters");
                }
            }

            if (propertyName == null || Utilities.GetPropertyName(() => this.Merchant) == propertyName)
            {
                if (!Utilities.ValidateRequiredString(this.Merchant))
                {
                    errors.Add("Merchant is required");
                }
                if (!Utilities.ValidateStringLength(this.Merchant, 32))
                {
                    errors.Add("Merchant cannot be greater than 32 characters");
                }
            }

            if (propertyName == null || Utilities.GetPropertyName(() => this.BilledAmount) == propertyName)
            {
                if (!Utilities.ValidatePositive(this.BilledAmount))
                {
                    errors.Add("Billed Amount must be a positive number");
                }
            }

            if (propertyName == null || Utilities.GetPropertyName(() => this.TransactionAmount) == propertyName)
            {
                if (!Utilities.ValidatePositive(this.TransactionAmount))
                {
                    errors.Add("Transaction Amount must be a positive number");
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
    }
}
