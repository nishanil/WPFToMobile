using Expenses.WPF.ExpensesService;
using Expenses.WPF.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IServiceFactory _serviceFactory;
        private IViewService _viewService;
        private INavigationService _navigationService;
        private IRepositoryService _repositoryService;
        private ICurrentIdentityService _currentIdentityService;

        public ICommand CloseCommand
        {
            get
            {
                if (this._closeCommand == null)
                {
                    this._closeCommand = new RelayCommand(() => this.OnRequestClose());
                }
                return this._closeCommand;
            }
        }
        private ICommand _closeCommand;

        public event EventHandler RequestClose;
        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public ICommand ShowChargesCommand { get; private set; }
        public ICommand NewChargeCommand { get; private set; }

        public ICommand NewReportCommand { get; private set; }
        public ICommand ShowSavedReportsCommand { get; private set; }
        public ICommand ShowSubmittedReportsCommand { get; private set; }
        public ICommand ShowApprovedReportsCommand { get; private set; }
        public ICommand ResetDataCommand { get; private set; }

        public MainWindowViewModel(IServiceFactory serviceFactory)
        {
            this._serviceFactory = serviceFactory;
            this._currentIdentityService = serviceFactory.CurrentIdentityService;
            this._viewService = serviceFactory.ViewService;
            this._navigationService = serviceFactory.NavigationService;
            this._repositoryService = serviceFactory.RepositoryService;

            this.NewChargeCommand = new RelayCommand(() => this.NewCharge());
            this.ShowChargesCommand = new RelayCommand(() => this.ShowChargesAsync());
            this.NewReportCommand = new RelayCommand(async () => await this.NewReport());
            this.ShowSavedReportsCommand = new RelayCommand(async () => await this.ShowSavedReportsAsync());
            this.ShowSubmittedReportsCommand = new RelayCommand(async () => await this.ShowSubmittedReportsAsync());
            this.ShowApprovedReportsCommand = new RelayCommand(async () => await this.ShowApprovedReportsAsync());
            this.ResetDataCommand = new RelayCommand(async () => await this.ResetDataAsync());
        }

        public ViewModelBase CurrentViewModel
        {
            get { return this._currentViewModel; }
            set
            {
                if (this._currentViewModel == value) { return; }

                this._currentViewModel = value;
                this.NotifyOfPropertyChange(() => this.CurrentViewModel);
            }
        }
        private ViewModelBase _currentViewModel;

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this._isBusy = value;
                this.NotifyOfPropertyChange(() => this.IsBusy);
            }
        }
        private bool _isBusy;

        public string CurrentUserAlias
        {
            get
            {
                return this._currentUserAlias;
            }
            set
            {
                if (this._currentUserAlias == value) { return; }
                this._currentUserAlias = value;
                this.NotifyOfPropertyChange(() => this.CurrentUserAlias);
            }
        }
        private string _currentUserAlias;

        public int CurrentUserEmployeeId
        {
            get
            {
                return this._currentUserEmployeeId;
            }
            set
            {
                if (this._currentUserEmployeeId == value) { return; }
                this._currentUserEmployeeId = value;
                this.NotifyOfPropertyChange(() => this.CurrentUserEmployeeId);
            }
        }
        private int _currentUserEmployeeId;

        public string CurrentUserManager
        {
            get
            {
                return this._currentUserManager;
            }
            set
            {
                if (this._currentUserManager == value) { return; }
                this._currentUserManager = value;
                this.NotifyOfPropertyChange(() => this.CurrentUserManager);
            }
        }
        private string _currentUserManager;

        public void ShowCharge(ChargeViewModel charge)
        {
            this.CurrentViewModel = charge;
        }

        public void NewCharge()
        {
            this.ShowCharge(
                new ChargeViewModel(new Charge()
                    {
                        EmployeeId = this.CurrentUserEmployeeId,
                        ExpenseDate = DateTime.Today,
                    }, 
                    this._serviceFactory)
                );
        }

        public void ShowChargesAsync()
        {
            ChargesViewModel vm = new ChargesViewModel(this._serviceFactory);
            this._viewService.ExecuteBusyActionAsync<Charge[]>(() =>
                {
                    return this._repositoryService.GetOutstandingChargesForEmployeeAsync(this.CurrentUserEmployeeId);
                }).ContinueWith((charges) =>
                {
                    var outstandingCharges = from charge in charges.Result
                                             select new ChargeViewModel(charge, this._serviceFactory);

                    vm.LoadCharges(outstandingCharges);
                    this.CurrentViewModel = vm;
                });
        }

        public async Task NewReport()
        {
            ExpenseReportViewModel reportVM = new ExpenseReportViewModel(this._serviceFactory)
            {
                Approver = this.CurrentUserManager,
                EmployeeId = this.CurrentUserEmployeeId,
                ExpenseReportId = 0,
            };

            await this.ShowExpenseReportAsync(reportVM);
        }

        public async Task ShowExpenseReportAsync(ExpenseReportViewModel expenseReportViewModel)
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    var editReportVM = new EditExpenseReportViewModel(this._serviceFactory);
                    editReportVM.ExpenseReport = expenseReportViewModel;

                    AddChargesViewModel addChargesVM = new AddChargesViewModel(this._serviceFactory);
                    await addChargesVM.LoadChargesAsync();
                    editReportVM.AddCharges = addChargesVM;

                    ExpenseReportChargesViewModel associatedChargesVM = new ExpenseReportChargesViewModel(this._serviceFactory);
                    await associatedChargesVM.LoadChargesAsync(expenseReportViewModel.ExpenseReportId);
                    editReportVM.AssociatedCharges = associatedChargesVM;

                    this.CurrentViewModel = editReportVM;
                });
        }

        public async Task ShowSavedReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // get saved expense reports
                    ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel(this._serviceFactory);
                    await expenseReportsViewModel.LoadSavedExpenseReportsAsync();
                    this.CurrentViewModel = expenseReportsViewModel;
                });
        }

        public async Task ShowSubmittedReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // show submitted reports
                    ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel(this._serviceFactory);
                    await expenseReportsViewModel.LoadSubmittedExpenseReportsAsync();
                    this.CurrentViewModel = expenseReportsViewModel;
                });
        }

        public async Task ShowApprovedReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // show approved reports
                    ExpenseReportsViewModel expenseReportsViewModel = new ExpenseReportsViewModel(this._serviceFactory);
                    await expenseReportsViewModel.LoadApprovedExpenseReportsAsync();
                    this.CurrentViewModel = expenseReportsViewModel;
                });
        }

        public async Task ResetDataAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // reset data
                    await this._repositoryService.ResetDataAsync();

                    // load up data of default employee
                    var employee = await this._repositoryService.GetEmployeeAsync(App.DefaultEmployeeAlias);
                    this._currentIdentityService.SetNewIdentity(
                        employee.Alias,
                        employee.EmployeeId,
                        employee.Manager,
                        employee.Name, 
                        false);

                    this.ShowChargesAsync();
                });
        }

        internal void OnCurrentIdentityChanged()
        {
            this.CurrentUserAlias = this._currentIdentityService.Alias;
            this.CurrentUserEmployeeId = this._currentIdentityService.EmployeeId;
            this.CurrentUserManager = this._currentIdentityService.Manager;
        }
    }
}
