using System;
using System.Collections.Generic;
using System.Text;

namespace MyExpenses.Models
{
    public enum ExpenseReportStatus
    {
        Submitted,
        Saved,
        Approved
    }

    public class ExpenseReport : BaseDataObject
    {


        public string EmployeeId { get; set; }


        public ExpenseReportStatus Status { get; set; }


        public decimal Amount { get; set; }


        public int CostCenter { get; set; }


        public string Notes { get; set; }


        public string Approver { get; set; }


        public DateTime? DateSubmitted { get; set; }


        public DateTime? DateResolved { get; set; }
    }
}
