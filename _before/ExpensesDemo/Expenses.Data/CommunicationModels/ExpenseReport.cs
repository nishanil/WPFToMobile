using System;
using System.Runtime.Serialization;

namespace Expenses.Data.CommunicationModels
{
    [DataContract]
    public enum ExpenseReportStatus
    {
        [EnumMember]
        Submitted,
        [EnumMember]
        Saved,
        [EnumMember]
        Approved
    }

    [DataContract]
    public class ExpenseReport
    {
        public ExpenseReport()
        {}

        public ExpenseReport(DbExpenseReport dbExpenseReport)
        {
            this.Amount = dbExpenseReport.Amount;
            this.Approver = dbExpenseReport.Approver;
            this.CostCenter = dbExpenseReport.CostCenter;
            this.DateResolved = dbExpenseReport.DateResolved;
            this.DateSubmitted = dbExpenseReport.DateSubmitted;
            this.EmployeeId = dbExpenseReport.EmployeeId;
            this.ExpenseReportId = dbExpenseReport.ExpenseReportId;
            this.Notes = dbExpenseReport.Notes;
            this.Status = (ExpenseReportStatus)dbExpenseReport.Status;
        }

        [DataMember]
        public int ExpenseReportId { get; set; }

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public ExpenseReportStatus Status { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int CostCenter { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public string Approver { get; set; }

        [DataMember]
        public DateTime DateSubmitted { get; set; }

        [DataMember]
        public DateTime DateResolved { get; set; }
    }
}
