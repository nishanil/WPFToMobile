
using System;
using System.Threading.Tasks;
using MyExpenses.Models;
using Xamarin.Forms;
using MyExpenses.DataStores;
using System.Linq;

namespace Expenses.WPF.Services
{
    public class RepositoryService : IRepositoryService
    {
        private IViewService _viewService = DependencyService.Get<IViewService>();

        public async Task<string> CreateNewChargeAsync(Charge charge)
        {
            try
            {

                if (await DependencyService.Get<ChargeDataStore>().AddItemAsync(charge))
                {
                    return charge.Id;
                }
            }

            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<string> CreateNewExpenseReportAsync(ExpenseReport expenseReport)
        {
            try
            {
                if (await DependencyService.Get<ExpenseReportDataStore>().AddItemAsync(expenseReport))
                {
                    return expenseReport.Id;
                }
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task DeleteExpenseReportAsync(string expenseReportId)
        {
            try
            {
                var report = await DependencyService.Get<ExpenseReportDataStore>().GetItemsAsync();
                var expReport = report.SingleOrDefault(i => i.Id == expenseReportId);

                await DependencyService.Get<ExpenseReportDataStore>().DeleteItemAsync(expReport);

            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }
        }

        public async Task<Charge> GetChargeAsync(string chargeId)
        {
            try
            {
                var allCharges  = await DependencyService.Get<ChargeDataStore>().GetItemsAsync();
                return allCharges?.SingleOrDefault(i => i.Id == chargeId);
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<Charge[]> GetChargesForExpenseReportAsync(string expenseReportId)
        {
            try
            {
                var allCharges = await DependencyService.Get<ChargeDataStore>().GetItemsAsync();
                var charges = allCharges.Where(i => i.ExpenseReportId == expenseReportId);
                return charges?.ToArray();
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<Employee> GetEmployeeAsync(string employeeAlias)
        {
            try
            {
                var allEmployees= await DependencyService.Get<EmployeeDataStore>().GetItemsAsync();
                return allEmployees?.SingleOrDefault(i => i.Alias == employeeAlias);
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<ExpenseReport> GetExpenseReportAsync(string expenseReportId)
        {
            try
            {
                var allExpenses = await DependencyService.Get<ExpenseReportDataStore>().GetItemsAsync();
                return allExpenses?.SingleOrDefault(i => i.Id == expenseReportId);
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<ExpenseReport[]> GetExpenseReportsForEmployeeAsync(string employeeId)
        {
            try
            {
                var allExpenses = await DependencyService.Get<ExpenseReportDataStore>().GetItemsAsync();
                var expenses = allExpenses.Where(i => i.EmployeeId == employeeId);
                return expenses?.ToArray();
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<ExpenseReport[]> GetExpenseReportsForEmployeeByStatusAsync(string employeeId, ExpenseReportStatus status)
        {
            try
            {
                var allExpenses = await DependencyService.Get<ExpenseReportDataStore>().GetItemsAsync();
                var expenses = allExpenses.Where(i => (i.EmployeeId == employeeId) && (i.Status == status));
                return expenses?.ToArray();
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }
            return null;
        }

        public async Task<Charge[]> GetOutstandingChargesForEmployeeAsync(string employeeId)
        {
            try
            {
                //TODO: Verify Guid.Empty may not be required
                var allCharges = await DependencyService.Get<ChargeDataStore>().GetItemsAsync();
                var charges = allCharges.Where(i => (i.EmployeeId == employeeId) && 
                (string.IsNullOrEmpty(i.ExpenseReportId)));
                return charges?.ToArray();
            }
            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public Task ResetDataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateChargeAsync(Charge charge)
        {
            try
            {

                if (await DependencyService.Get<ChargeDataStore>().UpdateItemAsync(charge))
                {
                    return charge.Id;
                }
            }

            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }

        public async Task<string> UpdateExpenseReportAsync(ExpenseReport expenseReport)
        {
            try
            {

                if (await DependencyService.Get<ExpenseReportDataStore>().UpdateItemAsync(expenseReport))
                {
                    return expenseReport.Id;
                }
            }

            catch (Exception ex)
            {
                await this._viewService.ShowError(String.Format("Error in API call: {0}", ex.Message));
            }

            return null;
        }
    }
    //TODO: Fix this
    #region Legacy Code
    //class RepositoryService : IRepositoryService
    //{
    //    private IViewService _viewService;

    //    public RepositoryService(IViewService viewService)
    //    {
    //        this._viewService = viewService;
    //    }

    //    public async Task<Employee> GetEmployeeAsync(string employeeAlias)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {

    //                string theAlias = employeeAlias.ToLower();
    //                return await client.GetEmployeeAsync(theAlias);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new Employee();
    //    }


    //    public async Task<Charge[]> GetOutstandingChargesForEmployeeAsync(int employeeId)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.GetOutstandingChargesForEmployeeAsync(employeeId);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new Charge[0];
    //    }

    //    public async Task<Charge[]> GetChargesForExpenseReportAsync(int expenseReportId)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.GetChargesForExpenseReportAsync(expenseReportId);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new Charge[0];
    //    }

    //    public async Task<ExpenseReport[]> GetExpenseReportsForEmployeeAsync(int employeeId)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.GetExpenseReportsForEmployeeAsync(employeeId);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new ExpenseReport[0];
    //    }

    //    public async Task<ExpenseReport[]> GetExpenseReportsForEmployeeByStatusAsync(int employeeId, ExpenseReportStatus status)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.GetExpenseReportsForEmployeeByStatusAsync(employeeId, status);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new ExpenseReport[0];
    //    }


    //    public async Task<Charge> GetChargeAsync(int chargeId)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.GetChargeAsync(chargeId);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new Charge();
    //    }

    //    public async Task<ExpenseReport> GetExpenseReportAsync(int expenseReportId)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.GetExpenseReportAsync(expenseReportId);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return new ExpenseReport();
    //    }

    //    public async Task<int> CreateNewChargeAsync(Charge charge)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.CreateNewChargeAsync(charge);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return 0;
    //    }

    //    public async Task<int> CreateNewExpenseReportAsync(ExpenseReport expenseReport)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.CreateNewExpenseReportAsync(expenseReport);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return 0;
    //    }

    //    public async Task<int> UpdateChargeAsync(Charge charge)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.UpdateChargeAsync(charge);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return 0;
    //    }

    //    public async Task<int> UpdateExpenseReportAsync(ExpenseReport expenseReport)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                return await client.UpdateExpenseReportAsync(expenseReport);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }

    //        return 0;
    //    }

    //    public async Task DeleteExpenseReportAsync(int expenseReportId)
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                await client.DeleteExpenseReportAsync(expenseReportId);
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }
    //    }

    //    public async Task ResetDataAsync()
    //    {
    //        try
    //        {
    //            using (var client = new ExpenseServiceClient())
    //            {
    //                await client.ResetDataAsync();
    //            }
    //        }
    //        catch (System.ServiceModel.EndpointNotFoundException)
    //        {
    //            this._viewService.ShowError("Could not connect to configured service.");
    //        }
    //        catch (Exception ex)
    //        {
    //            this._viewService.ShowError(String.Format("Error in WCF call: {0}", ex.Message));
    //        }
    //    }
    //}
    #endregion 

}
