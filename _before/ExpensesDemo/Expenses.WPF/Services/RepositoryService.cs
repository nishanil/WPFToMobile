using Expenses.WPF.ExpensesService;
using System;
using System.Threading.Tasks;

namespace Expenses.WPF.Services
{
    class RepositoryService : IRepositoryService
    {
        private IViewService _viewService;

        public RepositoryService(IViewService viewService)
        {
            this._viewService = viewService;
        }

        public async Task<Employee> GetEmployeeAsync(string employeeAlias)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {

                    string theAlias = employeeAlias.ToLower();
                    return await client.GetEmployeeAsync(theAlias);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new Employee();
        }


        public async Task<Charge[]> GetOutstandingChargesForEmployeeAsync(int employeeId)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.GetOutstandingChargesForEmployeeAsync(employeeId);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new Charge[0];
        }

        public async Task<Charge[]> GetChargesForExpenseReportAsync(int expenseReportId)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.GetChargesForExpenseReportAsync(expenseReportId);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new Charge[0];
        }

        public async Task<ExpenseReport[]> GetExpenseReportsForEmployeeAsync(int employeeId)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.GetExpenseReportsForEmployeeAsync(employeeId);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new ExpenseReport[0];
        }

        public async Task<ExpenseReport[]> GetExpenseReportsForEmployeeByStatusAsync(int employeeId, ExpenseReportStatus status)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.GetExpenseReportsForEmployeeByStatusAsync(employeeId, status);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new ExpenseReport[0];
        }


        public async Task<Charge> GetChargeAsync(int chargeId)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.GetChargeAsync(chargeId);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new Charge();
        }

        public async Task<ExpenseReport> GetExpenseReportAsync(int expenseReportId)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.GetExpenseReportAsync(expenseReportId);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return new ExpenseReport();
        }

        public async Task<int> CreateNewChargeAsync(Charge charge)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.CreateNewChargeAsync(charge);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return 0;
        }

        public async Task<int> CreateNewExpenseReportAsync(ExpenseReport expenseReport)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.CreateNewExpenseReportAsync(expenseReport);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return 0;
        }

        public async Task<int> UpdateChargeAsync(Charge charge)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.UpdateChargeAsync(charge);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return 0;
        }

        public async Task<int> UpdateExpenseReportAsync(ExpenseReport expenseReport)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    return await client.UpdateExpenseReportAsync(expenseReport);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }

            return 0;
        }

        public async Task DeleteExpenseReportAsync(int expenseReportId)
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    await client.DeleteExpenseReportAsync(expenseReportId);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }
        }

        public async Task ResetDataAsync()
        {
            try
            {
                using (var client = new ExpenseServiceClient())
                {
                    await client.ResetDataAsync();
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                this._viewService.ShowError("Could not connect to configured service.");
            }
            catch (Exception ex)
            {
                this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
            }
        }
    }
}
