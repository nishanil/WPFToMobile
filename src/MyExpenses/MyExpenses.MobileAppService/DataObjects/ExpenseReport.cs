using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyExpenses.MobileAppService.DataObjects
{
    public class ExpenseReport : EntityData
    {
        public enum ExpenseReportStatus
        {
            Submitted,
            Saved,
            Approved
        }

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