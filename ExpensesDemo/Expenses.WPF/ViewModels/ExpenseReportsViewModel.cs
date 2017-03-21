using Expenses.WPF.ExpensesService;
using Expenses.WPF.Misc;
using Expenses.WPF.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.WPF.ViewModels
{
    public class ExpenseReportsViewModel : ViewModelBase
    {
        private IServiceFactory _serviceFactory;
        public ObservableCollection<GroupInfoList<object>> GroupedExpenseReports
        {
            get { return this._groupedExpenseReports; }
            set
            {
                this._groupedExpenseReports = value;
                this.NotifyOfPropertyChange(() => this.GroupedExpenseReports);
            }
        }
        private ObservableCollection<GroupInfoList<object>> _groupedExpenseReports;

        public ObservableCollection<ExpenseReportViewModel> ExpenseReports
        {
            get { return this._expenseReports; }
            set
            {
                this._expenseReports = value;
                this.NotifyOfPropertyChange(() => this.ExpenseReports);
            }
        }
        private ObservableCollection<ExpenseReportViewModel> _expenseReports;

        public readonly IRepositoryService _repositoryService;
        public readonly INavigationService _navigationService;
        public readonly ICurrentIdentityService _currentIdentityService;
        private readonly IViewService _viewService;

        public ICommand ViewReportCommand { get; private set; }


        public ExpenseReportsViewModel(IServiceFactory serviceFactory)
        {
            this._serviceFactory = serviceFactory;
            this._currentIdentityService = serviceFactory.CurrentIdentityService;
            this._navigationService = serviceFactory.NavigationService;
            this._repositoryService = serviceFactory.RepositoryService;
            this._viewService = serviceFactory.ViewService;

            this._expenseReports = new ObservableCollection<ExpenseReportViewModel>();
            this._groupedExpenseReports = new ObservableCollection<GroupInfoList<object>>();

            this.ViewReportCommand = new RelayCommand<ExpenseReportViewModel>(
                (report) =>
                {
                    this.ViewReport(report);
                });
        }

        private void ViewReport(ExpenseReportViewModel reportViewModel)
        {
            this._navigationService.ShowExpenseReport(reportViewModel);
        }

        protected void Load(IEnumerable<ExpenseReport> expenseReports)
        {
            this.ExpenseReports.Clear();
            foreach (ExpenseReport expenseReport in expenseReports)
            {
                this.ExpenseReports.Add(new ExpenseReportViewModel(expenseReport, this._serviceFactory));
            }
            base.NotifyOfPropertyChange(() => this.ExpenseReports);
        }

        public async Task LoadAllExpenseReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repositoryService.GetExpenseReportsForEmployeeAsync(this._currentIdentityService.EmployeeId));
                });
        }

        public async Task LoadSavedExpenseReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repositoryService.GetExpenseReportsForEmployeeByStatusAsync(this._currentIdentityService.EmployeeId, ExpenseReportStatus.Saved));
                });
        }

        public async Task LoadSubmittedExpenseReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repositoryService.GetExpenseReportsForEmployeeByStatusAsync(this._currentIdentityService.EmployeeId, ExpenseReportStatus.Submitted));
                });
        }

        public async Task LoadApprovedExpenseReportsAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    this.Load(await this._repositoryService.GetExpenseReportsForEmployeeByStatusAsync(this._currentIdentityService.EmployeeId, ExpenseReportStatus.Approved));
                });
        }
    }
}
