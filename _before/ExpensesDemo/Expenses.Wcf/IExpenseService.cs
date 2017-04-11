using Expenses.Data.CommunicationModels;
using System.Collections.Generic;
using System.ServiceModel;

namespace Expenses.Wcf
{
    [ServiceContract]
    public interface IExpenseService
    {
        [OperationContract]
        Employee GetEmployee(string employeeAlias);

        [OperationContract]
        Charge GetCharge(int chargeId);

        [OperationContract]
        ExpenseReport GetExpenseReport(int expenseReportId);

        [OperationContract]
        ICollection<Charge> GetOutstandingChargesForEmployee(int employeeId);

        [OperationContract]
        ICollection<Charge> GetChargesForExpenseReport(int expenseReportId);

        [OperationContract]
        ICollection<ExpenseReport> GetExpenseReportsForEmployee(int employeeId);

        [OperationContract]
        ICollection<ExpenseReport> GetExpenseReportsForEmployeeByStatus(int employeeId, ExpenseReportStatus status);

        [OperationContract]
        int CreateNewCharge(Charge charge);

        [OperationContract]
        int CreateNewExpenseReport(ExpenseReport expenseReport);

        [OperationContract]
        int UpdateCharge(Charge charge);

        [OperationContract]
        int UpdateExpenseReport(ExpenseReport expenseReport);

        [OperationContract]
        void DeleteExpenseReport(int expenseReportId);

        [OperationContract]
        void ResetData();
    }
}
