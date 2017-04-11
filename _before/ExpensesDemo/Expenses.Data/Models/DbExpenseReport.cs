using Expenses.Data.CommunicationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Expenses.Data
{
    [DataContract(Name = "ExpenseReportStatus")]
    public enum DbExpenseReportStatus
    {
        [EnumMember]
        Submitted,
        [EnumMember]
        Saved,
        [EnumMember]
        Approved
    }

    [DataContract(Name = "ExpenseReport", IsReference = true)]
    public class DbExpenseReport
    {
        public DbExpenseReport()
        {
            this.Charges = new HashSet<DbCharge>();
        }

        public DbExpenseReport(ExpenseReport expenseReport)
        {
            this.Amount = expenseReport.Amount;
            this.Approver = expenseReport.Approver;
            this.CostCenter = expenseReport.CostCenter;
            this.DateResolved = expenseReport.DateResolved;
            this.DateSubmitted = expenseReport.DateSubmitted;
            this.EmployeeId = expenseReport.EmployeeId;
            this.ExpenseReportId = expenseReport.ExpenseReportId;
            this.Notes = expenseReport.Notes;
            this.Status = (DbExpenseReportStatus)expenseReport.Status;

            this.Charges = new HashSet<DbCharge>();
        }

        [DataMember]
        [Key]
        public int ExpenseReportId { get; set; }
        [DataMember]
        public int EmployeeId { get; set; }
        public virtual DbEmployee Employee { get; set; }
        [DataMember]
        public virtual ICollection<DbCharge> Charges { get; set; }
        [DataMember]
        public DbExpenseReportStatus Status { get; set; }
        [DataMember]
        [Column(TypeName = "smallmoney")]
        public decimal Amount { get; set; }
        [DataMember]
        public int CostCenter { get; set; }
        [DataMember]
        [Required]
        [StringLength(250)]
        public string Notes { get; set; }
        [DataMember]
        [Required]
        [StringLength(25)]
        public string Approver { get; set; }
        [DataMember]
        [Column(TypeName = "date")]
        public DateTime DateSubmitted { get; set; }
        [DataMember]
        [Column(TypeName = "date")]
        public DateTime DateResolved { get; set; }
    }
}
