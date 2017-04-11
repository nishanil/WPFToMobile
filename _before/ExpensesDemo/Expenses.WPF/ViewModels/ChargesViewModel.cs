using Expenses.WPF.ExpensesService;
using Expenses.WPF.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.WPF.ViewModels
{
    public class ChargesViewModel : ViewModelBase
    {
        private IServiceFactory _serviceFactory;

        public ObservableCollection<ChargeViewModel> Charges
        {
            get { return this._charges; }
            set
            {
                this._charges = value;
                this.NotifyOfPropertyChange(() => this.Charges);
            }
        }
        private ObservableCollection<ChargeViewModel> _charges;

        public ICommand ViewChargeCommand { get; private set; }
        private readonly INavigationService _navigationService;
        private readonly IViewService _viewService;
        private readonly IRepositoryService _repositoryService;
        private readonly ICurrentIdentityService _currentIdentityService;

        public ChargesViewModel(IServiceFactory serviceFactory)
        {
            this._serviceFactory = serviceFactory;
            this._navigationService = serviceFactory.NavigationService;
            this._viewService = serviceFactory.ViewService;
            this._repositoryService = serviceFactory.RepositoryService;
            this._currentIdentityService = serviceFactory.CurrentIdentityService;

            this.Charges = new ObservableCollection<ChargeViewModel>();

            this.ViewChargeCommand = new RelayCommand<ChargeViewModel>(
                (charge) =>
                {
                    this.ViewCharge(charge);
                });
        }

        public void LoadCharges(IEnumerable<ChargeViewModel> charges)
        {
            this.Charges = new ObservableCollection<ChargeViewModel>(charges);
        }

        private void ViewCharge(ChargeViewModel chargeViewModel)
        {
            this._navigationService.ShowCharge(chargeViewModel);
        }

        public async Task SaveChargesAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    foreach (var c in this.Charges)
                    {
                        // save charge
                        await this._repositoryService.UpdateChargeAsync(new Charge() 
                        {
                            BilledAmount = c.BilledAmount,
                            ChargeId = c.ChargeId,
                            Description = c.Description,
                            EmployeeId = c.EmployeeId,
                            ExpenseDate = c.ExpenseDate,
                            ExpenseReportId = c.ExpenseReportId,
                            Location = c.Location,
                            Merchant = c.Merchant,
                            Notes = c.Notes,
                            TransactionAmount = c.TransactionAmount,
                        });
                    }
                });
        }

        public async Task LoadChargesAsync()
        {
            await this._viewService.ExecuteBusyActionAsync(
                async () =>
                {
                    // load outstanding charges for current employee
                    var outstandingCharges = 
                        await this._repositoryService
                        .GetOutstandingChargesForEmployeeAsync(this._currentIdentityService.EmployeeId);
                    LoadChargesHelper(outstandingCharges);
                });
        }

        public async Task LoadChargesAsync(int expenseReportId)
        {
            if (expenseReportId != 0)
            {
                await this._viewService.ExecuteBusyActionAsync(
                    async () =>
                    {
                        // load charges for particular report Id
                        var reportCharges = await this._repositoryService
                            .GetChargesForExpenseReportAsync(expenseReportId);
                        LoadChargesHelper(reportCharges);
                    });
            }
        }

        private void LoadChargesHelper(Charge[] chargeList)
        {
            this.Charges = new ObservableCollection<ChargeViewModel>(
                chargeList.Select((c) => new ChargeViewModel(c, this._serviceFactory)));
        }
    }
}
